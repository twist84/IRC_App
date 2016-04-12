using System;
using System.Text.RegularExpressions;

using ChatSharp;
using IRC_Lib;

namespace IRC_App
{
    class Program
    {
        static void Main()
        {
            DEBUG.SHOW("Main start.\n");

            Init();

            DEBUG.SHOW("{0}\n{1}\n{2}\n{3}\n{4}\n", Settings.IRC_Serv, Settings.IRC_Chan, Settings.IRC_User, Settings.IRC_Nick, Settings.IRC_Pass);

            Client.Connect(Client.client);
            Client.ChannelRecieved(Client.client);
            Client.UserRecieved(Client.client);
            while (true) ;

            //DEBUG.SHOW("Main end.");
        }

        static void Init()
        {
            DEBUG.SHOW("Init start.\n");

            Settings.TimeOut = Int32.Parse(Settings.GetSetting("TimeOut"));

            Settings.IRC_Serv = Settings.GetSetting("Server");
            Settings.IRC_Chan = Settings.GetSetting("Channel");
            Settings.IRC_User = Settings.GetSetting("Username");
            Settings.IRC_Nick = Settings.GetSetting("Nickname");
            Settings.IRC_Pass = Settings.GetSetting("Password");

            DEBUG.SHOW("Init end.");
        }
    }

    class Client
    {
        private static IrcClient _client = new IrcClient(
            Settings.IRC_Serv,
            new IrcUser(
                Settings.IRC_Nick,
                Settings.IRC_User,
                Settings.IRC_Pass)
            );

        public static IrcClient client { get; } = _client;

        public static void Connect(IrcClient client)
        {
            DEBUG.SHOW("Connect start.");

            /*
            :NickServ!NickServ@snoonet/services/NickServ NOTICE SoonTM :Password accepted - you are now recognized.
            */

            client.ConnectionComplete += (s, e) => {
                client.NoticeRecieved += (t, f) => {
                    Console.WriteLine(f.Message.RawMessage);
                    /*if (f.Message.RawMessage.Contains("Password accepted"))
                        client.JoinChannel(Settings.IRC_Chan);*/
                };
            };

            client.ConnectAsync();

            DEBUG.SHOW("Connect end.");
        }

        public static void ChannelRecieved(IrcClient client)
        {
            DEBUG.SHOW("ChannelRecieved start.");

            client.ChannelTopicReceived += (s, e) =>
            {
                DEBUG.SHOW("ChannelTopicReceived start.");

                string Topic = e.Channel.Topic;

                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("{0}", Topic);
                Console.ResetColor();

                if (!(Topic.Contains("ftp://") || Topic.Contains("http://") || Topic.Contains("https://")))
                    Logger.Log(String.Format("{0}", Topic));
                else
                    Logger.Log(String.Format("The topic in channel {0} contains a Url not logging it", e.Channel.Name));

                DEBUG.SHOW("ChannelTopicReceived end.");
            };

            client.ChannelMessageRecieved += (s, e) =>
            {
                DEBUG.SHOW("ChannelMessageRecieved start.");

                string Nick = Regex.Split(e.PrivateMessage.User.Nick, "!")[0];
                string Message = e.PrivateMessage.Message;

                var channel = client.Channels[e.PrivateMessage.Source];

                if (Message.Contains(Settings.IRC_Nick) || Message.Contains(Settings.IRC_User))
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("[{0}] {1}: {2}", Settings.Date, Nick, Message);
                    Console.ResetColor();
                    Logger.Log(String.Format("[{0}] {1}: {2}", Settings.Date, Nick, Message));
                }
                else
                {
                    Console.WriteLine("[{0}] {1}: {2}", Settings.Date, Nick, Message);
                    Logger.Log(String.Format("[{0}] {1}: {2}", Settings.Date, Nick, Message));
                }

                if (Message.Equals(".list"))
                    channel.SendMessage(String.Format("Hai, {0}", Nick));

                for (int i = 0; i < Settings.Commands.Length / 2; i++)
                {
                    string s1 = Settings.Commands[i, 0];
                    string s2 = Settings.Commands[i, 1];

                    if (Message.StartsWith("!" + s1))
                    {
                        if (Settings.IsTimedOut == false)
                        {
                            channel.SendMessage(String.Format("{0}, {1}", Nick, s2));
                            Timeout.Start();
                        }
                    }
                }

                foreach (string Trigger in Settings.Triggers)
                    if (!Message.StartsWith("!") && Message.Contains(Trigger))
                        channel.SendMessage(String.Format("Hai, {0}", Nick));

                DEBUG.SHOW("ChannelMessageRecieved end.");
            };

            DEBUG.SHOW("ChannelRecieved end.");
        }

        public static void UserRecieved(IrcClient client)
        {
            DEBUG.SHOW("UserRecieved start.");

            client.UserJoinedChannel += (s, e) =>
            {
                DEBUG.SHOW("UserJoinedChannel start.");

                string Nick = Regex.Split(e.User.Nick, "!")[0];
                string Host = Regex.Split(e.User.Hostmask, "!")[1];
                var Channel = e.Channel;

                if (Nick.Equals(Regex.Split(client.User.Nick, "!")[0]))
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[{0}] Now talking on {1}", Settings.Date, Channel.Name);
                    Console.ResetColor();
                    Logger.Log(String.Format("[{0}] Now talking on {1}", Settings.Date, Channel.Name));
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("[{0}] {1} ({2}) has joined", Settings.Date, Nick, Host);
                    Console.ResetColor();
                    Logger.Log(String.Format("[{0}] {1} ({2}) has joined", Settings.Date, Nick, Host));
                }
                
                foreach (string User in Settings.Users)
                {
                    foreach (string mUser in User.Split('|')[0].Split(';'))
                        if (Nick.ToLower().Equals(mUser))
                        {
                            Channel.SendMessage(String.Format(User.Split('|')[1], mUser));
                            Console.WriteLine("Sent welcome message to {0}", mUser);
                        }
                }

                for (int i = 0; i < Settings.Users.Length / 2; i++)
                {
                    string User = Settings.Users[i, 0];
                    string WelcomeMessage = Settings.Users[i, 1];

                    if (Nick.Equals(User))
                    Channel.SendMessage(String.Format("{0}, {1}", Nick, WelcomeMessage));
                }

                if (Nick.ToLower().StartsWith("snoo") || Nick.ToLower().StartsWith("eldorito"))
                {
                    Channel.SendMessage(String.Format("Welcome {0}, What is your enquiry", Nick));
                    Console.WriteLine("Sent welcome message to {0}", Nick);
                }

                DEBUG.SHOW("UserJoinedChannel end.");
            };

            client.UserKicked += (s, e) =>
            {
                DEBUG.SHOW("UserKicked start.");

                string Kicker = Regex.Split(e.Kicker.Nick, "!")[0];
                string Kicked = Regex.Split(e.Kicked.Nick, "!")[0];
                string Channel = e.Channel.Name;
                string Reason = e.Reason;

                if (String.IsNullOrEmpty(Reason))
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[{0}] {1} has kicked {2} from {3}", Settings.Date, Kicker, Kicked, Channel);
                    Console.ResetColor();
                    Logger.Log(String.Format("[{0}] {1} has kicked {2} from {3}", Settings.Date, Kicker, Kicked, Channel));
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[{0}] {1} has kicked {2} from {3} (reason {4})", Settings.Date, Kicker, Kicked, Channel, Reason);
                    Console.ResetColor();
                    Logger.Log(String.Format("[{0}] {1} has kicked {2} from {3} (reason {4})", Settings.Date, Kicker, Kicked, Channel, Reason));
                }

                Console.WriteLine("DEBUG: UserKicked end.");
            };

            client.UserPartedChannel += (s, e) =>
            {
                DEBUG.SHOW("UserPartedChannel start.");

                string Nick = Regex.Split(e.User.Nick, "!")[0];

                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[{0}] {1} has left",
                    Settings.Date, Nick);
                Console.ResetColor();
                Logger.Log(String.Format("[{0}] {1} has left",
                    Settings.Date, Nick));

                DEBUG.SHOW("UserPartedChannel end.");
            };

            client.UserQuit += (s, e) =>
            {
                DEBUG.SHOW("UserPartedChannel start.");

                string Nick = Regex.Split(e.User.Nick, "!")[0];

                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("[{0}] {1} has quit",
                    Settings.Date, Nick);
                Console.ResetColor();
                Logger.Log(String.Format("[{0}] {1} has quit",
                    Settings.Date, Nick));

                DEBUG.SHOW("UserPartedChannel end.");
            };

            DEBUG.SHOW("UserRecieved end.");
        }
    }
}
