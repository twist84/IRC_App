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
            string Date = DateTime.Today.ToShortDateString();
            string Time = DateTime.Now.ToLongTimeString();

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
            string Date = DateTime.Now.ToLongTimeString();

            client.ChannelTopicReceived += (s, e) =>
            {
                string Topic = e.Channel.Topic;
                
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("{0}", Topic);
                Console.ResetColor();

                if (!(Topic.Contains("ftp://") || Topic.Contains("http://") || Topic.Contains("https://")))
                {
                    Logger.Log(String.Format("{0}", Topic));
                }
                else
                    Logger.Log(String.Format("The topic in channel {0} contains a Url not logging it", e.Channel.Name));

            };

            client.ChannelMessageRecieved += (s, e) =>
            {
                string Nick = Regex.Split(e.PrivateMessage.User.Nick, "!")[0];
                string Message = e.PrivateMessage.Message;

                var channel = client.Channels[e.PrivateMessage.Source];

                switch (Message.Contains(vars.IRC_Nick) || Message.Contains(vars.IRC_User))
                {
                    case true:
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("[{0}] {1}: {2}",
                            Date, Nick, Message);
                        Console.ResetColor();
                        Logger.Log(String.Format("[{0}] {1}: {2}",
                            Date, Nick, Message));
                        break;
                    case false:
                        Console.WriteLine("[{0}] {1}: {2}",
                            Date, Nick, Message);
                        Logger.Log(String.Format("[{0}] {1}: {2}",
                            Date, Nick, Message));
                        break;
                }

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
            string Date = DateTime.Now.ToLongTimeString();

            client.UserJoinedChannel += (s, e) =>
            {
                string Nick = Regex.Split(e.User.Nick, "!")[0];
                string Host = Regex.Split(e.User.Hostmask, "!")[1];
                var Channel = e.Channel;

                switch (Nick == vars.IRC_Nick)
                {
                    case true:
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[{0}] Now talking on {1}",
                            Date, Channel.Name);
                        Console.ResetColor();
                        Logger.Log(String.Format("[{0}] Now talking on {1}", 
                            Date, Channel.Name));
                        break;
                    case false:
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("[{0}] {1} ({2}) has joined",
                            Date, Nick, Host);
                        Console.ResetColor();
                        Logger.Log(String.Format("[{0}] {1} ({2}) has joined", 
                            Date, Nick, Host));
                        break;
                }

                switch (Nick.ToLower())
                {
                    case "thetwister":
                        Channel.SendMessage("Thank you based programmer for my existence!");
                        Console.WriteLine("Sent welcome message to {0}", Nick);
                        break;
                    case "shockfire":
                        Channel.SendMessage("theTwister made me a Synth!");
                        Console.WriteLine("Sent welcome message to {0}", Nick);
                        break;
                    case "dany":
                        Channel.SendMessage(String.Format("Sup {0}... Hows the current progress on [insert latest project here]!", Nick));
                        Console.WriteLine("Sent welcome message to {0}", Nick);
                        break;
                    case "camden":
                        Channel.SendMessage("Ayy!");
                        Console.WriteLine("Sent welcome message to {0}", Nick);
                        break;
                    case "deelekgolo":
                        Channel.SendMessage("A wild DEElekgolo appeared!");
                        Console.WriteLine("Sent welcome message to {0}", Nick);
                        break;
                    case "emoose":
                        Channel.SendMessage("Our lord and savior has returned!");
                        Console.WriteLine("Sent welcome message to {0}", Nick);
                        break;
                    case "alex231":
                        Channel.SendMessage("HUD master!");
                        Console.WriteLine("Sent welcome message to {0}", Nick);
                        break;
                    case "rabidsquabbit":
                        Channel.SendMessage("Host master!");
                        Console.WriteLine("Sent welcome message to {0}", Nick);
                        break;
                    case "divide":
                    case "g-money":
                    case "weion":
                        Channel.SendMessage("Host disciple!");
                        Console.WriteLine("Sent welcome message to {0}", Nick);
                        break;
                }

                if (Nick.ToLower().StartsWith("snoo") || Nick.ToLower().StartsWith("eldorito"))
                {
                    Channel.SendMessage(String.Format("Welcome {0}, What is your enquiry", Nick));
                    Console.WriteLine("Sent welcome message to {0}", Nick);
                }
            };

            client.UserKicked += (s, e) =>
            {
                string Kicker = Regex.Split(e.Kicker.Nick, "!")[0];
                string Kicked = Regex.Split(e.Kicked.Nick, "!")[0];
                string Channel = e.Channel.Name;
                string Reason = e.Reason;

                switch (Reason == null)
                {
                    case true:
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[{0}] {1} has kicked {2} from {3}",
                            Date, Kicker, Kicked, Channel);
                        Console.ResetColor();
                        Logger.Log(String.Format("[{0}] {1} has kicked {2} from {3}", 
                            Date, Kicker, Kicked, Channel));
                        break;
                    case false:
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[{0}] {1} has kicked {2} from {3} (reason {4})",
                            Date, Kicker, Kicked, Channel, Reason);
                        Console.ResetColor();
                        Logger.Log(String.Format("[{0}] {1} has kicked {2} from {3} (reason {4})", 
                            Date, Kicker, Kicked, Channel, Reason));
                        break;
                }
            };

            client.UserPartedChannel += (s, e) =>
            {
                string Nick = Regex.Split(e.User.Nick, "!")[0];

                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[{0}] {1} has left",
                    Date, Nick);
                Console.ResetColor();
                Logger.Log(String.Format("[{0}] {1} has left",
                    Date, Nick));
            };

            client.UserQuit += (s, e) =>
            {
                string Nick = Regex.Split(e.User.Nick, "!")[0];

                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("[{0}] {1} has quit",
                    Date, Nick);
                Console.ResetColor();
                Logger.Log(String.Format("[{0}] {1} has quit",
                    Date, Nick));
            };
        }
    }
}
