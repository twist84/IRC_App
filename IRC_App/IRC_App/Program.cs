using System;
using System.IO;
using System.Text.RegularExpressions;

using ChatSharp;
using IRC_App;
using IRC_Lib;

namespace IRC_App
{
    class Program
    {
        static void Main()
        {
            Client.Connect(Client.client, Properties.Settings.Default);
            Client.ChannelRecieved(Client.client, Properties.Settings.Default);
            Client.UserRecieved(Client.client, Properties.Settings.Default);

            while (true) ; // Waste CPU cycles
        }
    }

    class Logger
    {
        public static void Log(string Message)
        {
            string Dir = Directory.GetCurrentDirectory();
            string Date = DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day;
            string Time = DateTime.Now.ToString("hh:mm:ss tt");

            string L_Log = String.Format("{0}\\Log_{1}.txt", Dir, Date);
            string L_Url = String.Format("{0}\\Url_{1}.txt", Dir, Date);
            string L_Urll = String.Format("{0}\\Url_{1}.txt at Timestamp {1}", Dir, Time);

            if (Message.Contains("ftp://") || Message.Contains("http://") || Message.Contains("https://"))
            {
                File.AppendAllText(L_Url,Message + Environment.NewLine);
                File.AppendAllText(L_Log, L_Urll + Environment.NewLine);
            }
            else
                File.AppendAllText(L_Log,
                    Message + Environment.NewLine);
        }
    }
}

namespace IRC_Lib
{
    class Client
    {
        private static IrcClient _client = new IrcClient(IRC_App.Properties.Settings.Default.IRC_Serv,
                    new IrcUser(IRC_App.Properties.Settings.Default.IRC_Nick,
                    IRC_App.Properties.Settings.Default.IRC_User,
                    IRC_App.Properties.Settings.Default.IRC_Pass)
                    );

        public static IrcClient client
        {
            get
            {
                return _client;
            }
        }

        public static void Connect(IrcClient client, IRC_App.Properties.Settings vars)
        {
            client.ConnectionComplete += (s, e) => client.JoinChannel(vars.IRC_Chan);
            client.ConnectAsync();
        }
        
        public static void ChannelRecieved(IrcClient client, IRC_App.Properties.Settings vars)
        {
            client.ChannelTopicReceived += (s, e) =>
            {
                string Topic = e.Channel.Topic;

                Console.WriteLine("{0}", Topic);
                Logger.Log(String.Format("{0}", Topic));
            };

            client.ChannelMessageRecieved += (s, e) =>
            {
                string Date = DateTime.Now.ToString("hh:mm:ss tt");
                string Nick = Regex.Split(e.PrivateMessage.User.Nick, "!")[0];
                string Message = e.PrivateMessage.Message;

                var channel = client.Channels[e.PrivateMessage.Source];

                Console.WriteLine("[{0}] {1}: {2}", 
                    Date, Nick, Message);
                Logger.Log(String.Format("[{0}] {1}: {2}", 
                    Date, Nick, Message));

                switch (Message)
                {
                    case ".list":
                        channel.SendMessage(String.Format(", {0}",
                            Nick));
                        break;
                }

                foreach (string Command in Regex.Split(IRC_App.Properties.Settings.Default.Commands, (";")))
                {
                    if (Message != "!" + Command.Split('|')[0])
                        continue;
                    else
                    {
                        channel.SendMessage(String.Format("{0}, {1}",
                            Nick, Command.Split('|')[1]));
                        break;
                    }
                }

                foreach (string Ping in Regex.Split(IRC_App.Properties.Settings.Default.Triggers, (";")))
                {
                    if (!Message.Contains(Ping))
                    {
                        break;
                    }
                    else
                        channel.SendMessage(String.Format("Hai, {0}",
                            Nick));
                }
            };
        }

        public static void UserRecieved(IrcClient client, IRC_App.Properties.Settings vars)
        {
            client.UserJoinedChannel += (s, e) =>
            {
                string Date = DateTime.Now.ToString("hh:mm:ss tt");
                string Nick = Regex.Split(e.User.Nick, "!")[0];
                string Host = Regex.Split(e.User.Hostmask, "!")[1];
                string Channel = e.Channel.Name;

                switch (Nick == vars.IRC_Nick)
                {
                    case true:
                        Console.WriteLine("[{0}] Now talking on {1}", 
                            Date, Channel);
                        Logger.Log(String.Format("[{0}] Now talking on {1}", 
                            Date, Channel));
                        break;
                    case false:
                        Console.WriteLine("[{0}] {1} ({2}) has joined", 
                            Date, Nick, Host);
                        Logger.Log(String.Format("[{0}] {1} ({2}) has joined", 
                            Date, Nick, Host));
                        break;
                }
                switch (Nick)
                {
                    case "theTwister":
                        e.Channel.SendMessage("Thank you based programmer for my existence!");
                        break;
                    case "Shockfire":
                        e.Channel.SendMessage("theTwister made me a Synth!");
                        break;
                    case "dany":
                        e.Channel.SendMessage("Sup dany... Hows the current progress on [insert latest project here]!");
                        break;
                    case "Camden":
                        e.Channel.SendMessage("Ayy!");
                        break;
                    case "DEElekgolo":
                        e.Channel.SendMessage("A wild DEElekgolo has appeared!");
                        break;
                    case "emoose":
                        e.Channel.SendMessage("Our lord and savior has returned!");
                        break;
                }
            };

            client.UserKicked += (s, e) =>
            {
                string Date = DateTime.Now.ToString("hh:mm:ss tt");
                string Kicker = Regex.Split(e.Kicker.Nick, "!")[0];
                string Kicked = Regex.Split(e.Kicked.Nick, "!")[0];
                string Channel = e.Channel.Name;
                string Reason = e.Reason;

                switch (Reason == null)
                {
                    case true:
                        Console.WriteLine("[{0}] {1} has kicked {2} from {3}", 
                            Date, Kicker, Kicked, Channel);
                        Logger.Log(String.Format("[{0}] {1} has kicked {2} from {3}", 
                            Date, Kicker, Kicked, Channel));
                        break;
                    case false:
                        Console.WriteLine("[{0}] {1} has kicked {2} from {3} (reason {4})",
                            Date, Kicker, Kicked, Channel, Reason);
                        Logger.Log(String.Format("[{0}] {1} has kicked {2} from {3} (reason {4})", 
                            Date, Kicker, Kicked, Channel, Reason));
                        break;
                }
            };

            client.UserPartedChannel += (s, e) =>
            {
                string Date = DateTime.Now.ToString("hh:mm:ss tt");
                string Nick = Regex.Split(e.User.Nick, "!")[0];

                Console.WriteLine("[{0}] {1} has left",
                    Date, Nick);
                Logger.Log(String.Format("[{0}] {1} has left",
                    Date, Nick));
            };

            client.UserQuit += (s, e) =>
            {
                string Date = DateTime.Now.ToString("hh:mm:ss tt");
                string Nick = Regex.Split(e.User.Nick, "!")[0];

                Console.WriteLine("[{0}] {1} has quit",
                    Date, Nick);
                Logger.Log(String.Format("[{0}] {1} has quit",
                    Date, Nick));
            };
        }
    }
}