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

            if (Settings.IsDEBUG.Equals(true))
            {
                Client.Connect(Client.client);
                Debug(Client.client);
            }
            else
            {
                Client.Connect(Client.client);
                Client.Channel(Client.client);
                Client.User(Client.client);
                Client.Invite(Client.client);
            }
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

        private static void Debug(IrcClient client)
        {
            client.ChannelListRecieved += (s, e) => {
                DEBUG.SHOW(e.Channel.Mode);
                DEBUG.SHOW(e.Channel.Name);
                DEBUG.SHOW(e.Channel.Topic);
                DEBUG.SHOW(e.Channel.Users.ToString());
            };
            client.ChannelMessageRecieved += (s, e) => {
                DEBUG.SHOW(e.IrcMessage.Command);
                DEBUG.SHOW(e.IrcMessage.Parameters);
                DEBUG.SHOW(e.IrcMessage.Prefix);
                DEBUG.SHOW(e.IrcMessage.RawMessage);
                DEBUG.SHOW(e.PrivateMessage.IsChannelMessage.ToString());
                DEBUG.SHOW(e.PrivateMessage.Message);
                DEBUG.SHOW(e.PrivateMessage.Source.ToString());
                DEBUG.SHOW(e.PrivateMessage.User.ToString());
            };
            client.ChannelTopicReceived += (s, e) => {
                DEBUG.SHOW(e.Channel.Mode);
                DEBUG.SHOW(e.Channel.Name);
                DEBUG.SHOW(e.Channel.Topic);
                DEBUG.SHOW(e.Channel.Users.ToString());
                DEBUG.SHOW(e.OldTopic);
                DEBUG.SHOW(e.Topic);
            };
            client.ModeChanged += (s, e) => {
                DEBUG.SHOW(e.Change);
                DEBUG.SHOW(e.Target);
                DEBUG.SHOW(e.User.Channels.ToString());
                DEBUG.SHOW(e.User.Hostmask);
                DEBUG.SHOW(e.User.Hostname);
                DEBUG.SHOW(e.User.Mode);
                DEBUG.SHOW(e.User.Nick);
                DEBUG.SHOW(e.User.Password);
                DEBUG.SHOW(e.User.RealName);
                DEBUG.SHOW(e.User.User);
            };
            client.MOTDPartRecieved += (s, e) => DEBUG.SHOW(e.MOTD);
            client.MOTDRecieved += (s, e) => DEBUG.SHOW(e.MOTD);
            client.NetworkError += (s, e) => DEBUG.SHOW(e.SocketError.ToString());
            client.NickChanged += (s, e) => {
                DEBUG.SHOW(e.NewNick);
                DEBUG.SHOW(e.OldNick);
                DEBUG.SHOW(e.User.Channels.ToString());
                DEBUG.SHOW(e.User.Hostmask);
                DEBUG.SHOW(e.User.Hostname);
                DEBUG.SHOW(e.User.Mode);
                DEBUG.SHOW(e.User.Nick);
                DEBUG.SHOW(e.User.Password);
                DEBUG.SHOW(e.User.RealName);
                DEBUG.SHOW(e.User.User);
            };
            client.NickInUse += (s, e) => {
                DEBUG.SHOW(e.DoNotHandle.ToString());
                DEBUG.SHOW(e.InvalidNick);
                DEBUG.SHOW(e.NewNick);
            };
            client.NoticeRecieved += (s, e) => {
                DEBUG.SHOW(e.Message.Command);
                DEBUG.SHOW(e.Message.Parameters);
                DEBUG.SHOW(e.Message.Prefix);
                DEBUG.SHOW(e.Message.RawMessage);
                DEBUG.SHOW(e.Notice);
                DEBUG.SHOW(e.Source);
            };
            client.PrivateMessageRecieved += (s, e) => {
                DEBUG.SHOW(e.IrcMessage.Command);
                DEBUG.SHOW(e.IrcMessage.Parameters);
                DEBUG.SHOW(e.IrcMessage.Prefix);
                DEBUG.SHOW(e.IrcMessage.RawMessage);
                DEBUG.SHOW(e.PrivateMessage.IsChannelMessage.ToString());
                DEBUG.SHOW(e.PrivateMessage.Message);
                DEBUG.SHOW(e.PrivateMessage.Source.ToString());
                DEBUG.SHOW(e.PrivateMessage.User.ToString());
            };
            client.RawMessageRecieved += (s, e) => {
                DEBUG.SHOW(e.Message);
                DEBUG.SHOW(e.Outgoing.ToString());
            };
            client.RawMessageSent += (s, e) => {
                DEBUG.SHOW(e.Message);
                DEBUG.SHOW(e.Outgoing.ToString());
            };
            client.ServerInfoRecieved += (s, e) => {
                DEBUG.SHOW(e.ServerInfo.ChannelTypes.ToString());
                DEBUG.SHOW(e.ServerInfo.IsGuess.ToString());
                DEBUG.SHOW(e.ServerInfo.MaxAwayLength.ToString());
                DEBUG.SHOW(e.ServerInfo.MaxChannelNameLength.ToString());
                DEBUG.SHOW(e.ServerInfo.MaxChannelsPerUser.ToString());
                DEBUG.SHOW(e.ServerInfo.MaxKickCommentLength.ToString());
                DEBUG.SHOW(e.ServerInfo.MaxModesPerCommand.ToString());
                DEBUG.SHOW(e.ServerInfo.MaxNickLength.ToString());
                DEBUG.SHOW(e.ServerInfo.MaxTopicLength.ToString());
                DEBUG.SHOW(e.ServerInfo.Prefixes.ToString());
                DEBUG.SHOW(e.ServerInfo.SupportedChannelModes.ToString());
                DEBUG.SHOW(e.ServerInfo.SupportsBanExceptions.ToString());
                DEBUG.SHOW(e.ServerInfo.SupportsInviteExceptions.ToString());
            };
            client.UserJoinedChannel += (s, e) => {
                DEBUG.SHOW(e.Channel.Mode);
                DEBUG.SHOW(e.Channel.Name);
                DEBUG.SHOW(e.Channel.Topic);
                DEBUG.SHOW(e.Channel.Users.ToString());
                DEBUG.SHOW(e.User.Channels.ToString());
                DEBUG.SHOW(e.User.Hostmask);
                DEBUG.SHOW(e.User.Hostname);
                DEBUG.SHOW(e.User.Mode);
                DEBUG.SHOW(e.User.Nick);
                DEBUG.SHOW(e.User.Password);
                DEBUG.SHOW(e.User.RealName);
                DEBUG.SHOW(e.User.User);
            };
            client.UserKicked += (s, e) => {
                DEBUG.SHOW(e.Channel.Mode);
                DEBUG.SHOW(e.Channel.Name);
                DEBUG.SHOW(e.Channel.Topic);
                DEBUG.SHOW(e.Channel.Users.ToString());
                DEBUG.SHOW(e.Channel.UsersByMode.ToString());
                DEBUG.SHOW(e.Kicked.Channels.ToString());
                DEBUG.SHOW(e.Kicked.Hostmask);
                DEBUG.SHOW(e.Kicked.Hostname);
                DEBUG.SHOW(e.Kicked.Mode);
                DEBUG.SHOW(e.Kicked.Nick);
                DEBUG.SHOW(e.Kicked.Password);
                DEBUG.SHOW(e.Kicked.RealName);
                DEBUG.SHOW(e.Kicked.User);
                DEBUG.SHOW(e.Kicker.Channels.ToString());
                DEBUG.SHOW(e.Kicker.Hostmask);
                DEBUG.SHOW(e.Kicker.Hostname);
                DEBUG.SHOW(e.Kicker.Mode);
                DEBUG.SHOW(e.Kicker.Nick);
                DEBUG.SHOW(e.Kicker.Password);
                DEBUG.SHOW(e.Kicker.RealName);
                DEBUG.SHOW(e.Kicker.User);
            };
            client.UserMessageRecieved += (s, e) => {
                DEBUG.SHOW(e.IrcMessage.Command);
                DEBUG.SHOW(e.IrcMessage.Parameters);
                DEBUG.SHOW(e.IrcMessage.Prefix);
                DEBUG.SHOW(e.IrcMessage.RawMessage);
                DEBUG.SHOW(e.PrivateMessage.IsChannelMessage.ToString());
                DEBUG.SHOW(e.PrivateMessage.Message);
                DEBUG.SHOW(e.PrivateMessage.Source.ToString());
                DEBUG.SHOW(e.PrivateMessage.User.ToString());
            };
            client.UserPartedChannel += (s, e) => {
                DEBUG.SHOW(e.Channel.Mode);
                DEBUG.SHOW(e.Channel.Name);
                DEBUG.SHOW(e.Channel.Topic);
                DEBUG.SHOW(e.Channel.Users.ToString());
                DEBUG.SHOW(e.User.Channels.ToString());
                DEBUG.SHOW(e.User.Hostmask);
                DEBUG.SHOW(e.User.Hostname);
                DEBUG.SHOW(e.User.Mode);
                DEBUG.SHOW(e.User.Nick);
                DEBUG.SHOW(e.User.Password);
                DEBUG.SHOW(e.User.RealName);
                DEBUG.SHOW(e.User.User);
            };
            client.UserQuit += (s, e) => {
                DEBUG.SHOW(e.User.Channels.ToString());
                DEBUG.SHOW(e.User.Hostmask);
                DEBUG.SHOW(e.User.Hostname);
                DEBUG.SHOW(e.User.Mode);
                DEBUG.SHOW(e.User.Nick);
                DEBUG.SHOW(e.User.Password);
                DEBUG.SHOW(e.User.RealName);
                DEBUG.SHOW(e.User.User);
            };
            client.WhoIsReceived += (s, e) => {
                DEBUG.SHOW(e.WhoIsResponse.Channels);
                DEBUG.SHOW(e.WhoIsResponse.IrcOp.ToString());
                DEBUG.SHOW(e.WhoIsResponse.LoggedInAs);
                DEBUG.SHOW(e.WhoIsResponse.SecondsIdle.ToString());
                DEBUG.SHOW(e.WhoIsResponse.Server);
                DEBUG.SHOW(e.WhoIsResponse.ServerInfo);
                DEBUG.SHOW(e.WhoIsResponse.User.Channels.ToString());
                DEBUG.SHOW(e.WhoIsResponse.User.Hostmask);
                DEBUG.SHOW(e.WhoIsResponse.User.Hostname);
                DEBUG.SHOW(e.WhoIsResponse.User.Mode);
                DEBUG.SHOW(e.WhoIsResponse.User.Nick);
                DEBUG.SHOW(e.WhoIsResponse.User.Password);
                DEBUG.SHOW(e.WhoIsResponse.User.RealName);
                DEBUG.SHOW(e.WhoIsResponse.User.User);
            };
            DEBUG.SHOW("Debug end.");
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

            //:NickServ!NickServ@snoonet/services/NickServ NOTICE SoonTM :Password accepted - you are now recognized.

            client.ConnectionComplete += (s, e) => {
                client.NoticeRecieved += (t, f) => {
                    DEBUG.SHOW(f.Message.RawMessage);
                    if (f.Message.RawMessage.Contains("Password accepted"))
                        client.JoinChannel("##eldorito");//Settings.IRC_Chan);
                };
            };

            client.ConnectAsync();

            DEBUG.SHOW("Connect end.");
        }

        public static void Invite(IrcClient client)
        {
            //:theTwister!theTwister@user/theTwister INVITE SoonTM :#eldorito
            
            client.RawMessageRecieved += (s, e) =>
            {
                if (e.Message.Contains("INVITE"))
                {
                    string InviteFromUser = Regex.Split(Regex.Split(e.Message, ":")[1], "!")[0];
                    string InviteToChannel = Regex.Split(e.Message, ":")[2];

                    foreach (string Channel in Settings.ActiveChannels)
                        if (InviteToChannel.Equals(Channel))
                            Settings.IsAlreadyInChannel = true;

                    if (Settings.IsAlreadyInChannel.Equals(false))
                    {
                        client.JoinChannel(InviteToChannel);
                        client.SendMessage("Thanks for the invite", InviteFromUser);

                        Settings.IsInMultipleChannels = true;

                        Settings.IsAlreadyInChannel = false;
                        Settings.ActiveChannels.Add(InviteToChannel);

                        DEBUG.SHOW(String.Format("Accepted invite from: {0} to join channel: ", InviteFromUser, InviteToChannel));
                        Logger.Log(String.Format("Accepted invite from: {0} to join channel: ", InviteFromUser, InviteToChannel));
                    }
                }
            };
        }

        public static void Channel(IrcClient client)
        {
            DEBUG.SHOW("ChannelRecieved start.");

            #region client.ChannelTopicReceived
            client.ChannelTopicReceived += (s, e) =>
            {
                DEBUG.SHOW("ChannelTopicReceived start.");

                string Channel = e.Channel.Name;
                string Topic = e.Channel.Topic;

                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("{0}", Topic);
                Console.ResetColor();

                if (!(Topic.Contains("ftp://") || Topic.Contains("http://") || Topic.Contains("https://")))
                    Logger.Log(String.Format("{0}", Topic));
                else
                    Logger.Log(String.Format("The topic in channel {0} contains a Url not logging it", Channel));

                DEBUG.SHOW("ChannelTopicReceived end.");
            };
            #endregion

            #region client.ChannelMessageRecieved
            client.ChannelMessageRecieved += (s, e) =>
            {
                DEBUG.SHOW("ChannelMessageRecieved start.");

                string Channel = e.PrivateMessage.Source;
                string Nick = Regex.Split(e.PrivateMessage.User.Nick, "!")[0];
                string Message = e.PrivateMessage.Message;

                var channel = client.Channels[Channel];

                switch (Settings.IsInMultipleChannels)
                {
                    #region false
                    case false:
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
                        break;
                    #endregion
                    #region true
                    case true:
                        if (Message.Contains(Settings.IRC_Nick) || Message.Contains(Settings.IRC_User))
                        {
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine("[{0}] {1}: {2}: {3}", Settings.Date, Channel, Nick, Message);
                            Console.ResetColor();
                            Logger.Log(String.Format("[{0}] {1}: {2}: {3}", Settings.Date, Channel, Nick, Message));
                        }
                        else
                        {
                            Console.WriteLine("[{0}] {1}: {2}: {3}", Settings.Date, Channel, Nick, Message);
                            Logger.Log(String.Format("[{0}] {1}: {2}: {3}", Settings.Date, Channel, Nick, Message));
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
                        break;
                        #endregion
                }

                DEBUG.SHOW("ChannelMessageRecieved end.");
            };
            #endregion

            DEBUG.SHOW("ChannelRecieved end.");
        }

        public static void User(IrcClient client)
        {
            DEBUG.SHOW("UserRecieved start.");

            #region client.UserJoinedChannel
            client.UserJoinedChannel += (s, e) =>
            {
                DEBUG.SHOW("UserJoinedChannel start.");

                string Nick = Regex.Split(e.User.Nick, "!")[0];
                string Host = Regex.Split(e.User.Hostmask, "!")[1];
                var Channel = e.Channel;

                switch (Settings.IsInMultipleChannels)
                {
                    #region false
                    case false:
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
                            Console.WriteLine("[{0}] {1} ({2}) has joined channel: {3}", Settings.Date, Nick, Host, Channel.Name);
                            Console.ResetColor();
                            Logger.Log(String.Format("[{0}] {1} ({2}) has joined channel: {3}", Settings.Date, Nick, Host, Channel.Name));
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
                        break;
                    #endregion
                    #region true
                    case true:
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
                            Console.WriteLine("[{0}] {1} ({2}) has joined channel: {3}", Settings.Date, Nick, Host, Channel.Name);
                            Console.ResetColor();
                            Logger.Log(String.Format("[{0}] {1} ({2}) has joined channel: {3}", Settings.Date, Nick, Host, Channel.Name));
                        }

                        for (int i = 0; i < Settings.Users.Length / 2; i++)
                        {
                            string User = Settings.Users[i, 0];
                            string WelcomeMessage = Settings.Users[i, 1];

                            if (Nick.Equals(User))
                            {
                                Channel.SendMessage(String.Format("{0}, {1}", Nick, WelcomeMessage));
                                Console.WriteLine("{0}: Sent welcome message to {1}", Channel.Name, Nick);
                            }
                        }

                        if (Nick.ToLower().StartsWith("snoo") || Nick.ToLower().StartsWith("eldorito"))
                        {
                            Channel.SendMessage(String.Format("Welcome {0}, What is your enquiry", Nick));
                            Console.WriteLine("{0}: Sent welcome message to {1}", Channel.Name, Nick);
                        }
                        break;
                        #endregion
                }

                DEBUG.SHOW("UserJoinedChannel end.");
            };
            #endregion
            #region client.UserPartedChannel
            client.UserPartedChannel += (s, e) =>
            {
                DEBUG.SHOW("UserPartedChannel start.");

                string Channel = e.Channel.Name;
                string Nick = Regex.Split(e.User.Nick, "!")[0];

                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[{0}] {1}: {2} has left", Settings.Date, Channel, Nick);
                Console.ResetColor();
                Logger.Log(String.Format("[{0}] {1} has left {2}", Settings.Date, Nick, Channel));

                DEBUG.SHOW("UserPartedChannel end.");
            };
            #endregion

            #region client.UserKicked
            client.UserKicked += (s, e) =>
            {
                DEBUG.SHOW("UserKicked start.");

                string Kicker = Regex.Split(e.Kicker.Nick, "!")[0];
                string Kicked = Regex.Split(e.Kicked.User, "!")[0];
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
            #endregion
            #region client.UserQuit
            client.UserQuit += (s, e) =>
            {
                DEBUG.SHOW("UserQuit start.");

                string Nick = Regex.Split(e.User.Nick, "!")[0];

                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("[{0}] {1} has quit", Settings.Date, Nick);
                Console.ResetColor();
                Logger.Log(String.Format("[{0}] {1} has quit", Settings.Date, Nick));

                DEBUG.SHOW("UserQuit end.");
            };
            #endregion

            #region client.UserMessageRecieved
            client.UserMessageRecieved += (s, e) =>
            {
                DEBUG.SHOW("UserMessageRecieved start.");

                string Channel = e.PrivateMessage.Source;
                string Nick = Regex.Split(e.PrivateMessage.User.Nick, "!")[0];
                string Message = e.PrivateMessage.Message;

                switch (Settings.IsInMultipleChannels)
                {
                    #region false
                    case false:
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
                            client.SendMessage(String.Format("Hai, {0}", Nick), e.PrivateMessage.Source);

                        for (int i = 0; i < Settings.Commands.Length / 2; i++)
                        {
                            string s1 = Settings.Commands[i, 0];
                            string s2 = Settings.Commands[i, 1];

                            if (Message.StartsWith("!" + s1))
                            {
                                if (Settings.IsTimedOut == false)
                                {
                                    client.SendMessage(String.Format("{0}, {1}", Nick, s2), e.PrivateMessage.Source);
                                }
                            }
                        }

                        foreach (string Trigger in Settings.Triggers)
                            if (!Message.StartsWith("!") && Message.Contains(Trigger))
                                client.SendMessage(String.Format("Hai, {0}", Nick), e.PrivateMessage.Source);
                        break;
                    #endregion
                    #region true
                    case true:
                        if (Message.Contains(Settings.IRC_Nick) || Message.Contains(Settings.IRC_User))
                        {
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine("[{0}] {1}: {2}: {3}", Settings.Date, Channel, Nick, Message);
                            Console.ResetColor();
                            Logger.Log(String.Format("[{0}] {1}: {2}: {3}", Settings.Date, Channel, Nick, Message));
                        }
                        else
                        {
                            Console.WriteLine("[{0}] {1}: {2}: {3}", Settings.Date, Channel, Nick, Message);
                            Logger.Log(String.Format("[{0}] {1}: {2}: {3}", Settings.Date, Channel, Nick, Message));
                        }

                        if (Message.Equals(".list"))
                            client.SendMessage(String.Format("Hai, {0}", Nick), e.PrivateMessage.Source);

                        for (int i = 0; i < Settings.Commands.Length / 2; i++)
                        {
                            string s1 = Settings.Commands[i, 0];
                            string s2 = Settings.Commands[i, 1];

                            if (Message.StartsWith("!" + s1))
                            {
                                if (Settings.IsTimedOut == false)
                                {
                                    client.SendMessage(String.Format("{0}, {1}", Nick, s2), e.PrivateMessage.Source);
                                }
                            }
                        }

                        foreach (string Trigger in Settings.Triggers)
                            if (!Message.StartsWith("!") && Message.Contains(Trigger))
                                client.SendMessage(String.Format("Hai, {0}", Nick), e.PrivateMessage.Source);
                        break;
                        #endregion
                }

                DEBUG.SHOW("UserMessageRecieved end.");
            };
            #endregion

            DEBUG.SHOW("UserRecieved end.");
        }
    }
}
