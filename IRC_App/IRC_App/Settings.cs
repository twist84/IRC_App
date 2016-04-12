using System;
using System.IO;

namespace IRC_Lib
{
    public class Settings
    {
        public static string[] Default { get; } = { "30", "irc.freenode.net", "#botwar", "IRC_Bot", "Password" };

        public static string GetSetting(string Setting)
        {
            var IniFile = new IniFile.IniFile("Settings.ini");

            switch (Setting)
            {
                case "Server":
                case "Channel":
                case "Username":
                case "Nickname":
                case "Password":
                    if (!IniFile.KeyExists(Setting, "Settings.IRC"))
                    {
                        if (Setting.Equals("Server"))
                            IniFile.Write(Setting, Default[1], "Settings.IRC");
                        else if (Setting.Equals("Channel"))
                            IniFile.Write(Setting, Default[2], "Settings.IRC");
                        else if (Setting.Equals("Username") || Setting.Equals("Nickname"))
                            IniFile.Write(Setting, Default[3], "Settings.IRC");
                        else if (Setting.Equals("Password"))
                            IniFile.Write(Setting, Default[4], "Settings.IRC");
                        return IniFile.Read(Setting, "Settings.IRC");
                    }
                    else
                        return IniFile.Read(Setting, "Settings.IRC");

                case "TimeOut":
                    if (!IniFile.KeyExists(Setting, "Settings.Other"))
                    {
                        IniFile.Write(Setting, Default[0], "Settings.Other");
                        return IniFile.Read(Setting, "Settings.Other");
                    }
                    else
                        return IniFile.Read(Setting, "Settings.Other");
            }
            return null;
        }

        
        public static bool IsDEBUG { get; } = false;


        public static bool IsLogging { get; } = false;


        public static int TimeOut { get; set; } = Int32.Parse(Default[0]);

        public static bool IsTimedOut { get; set; } = false;


        public static string IRC_Serv { get; set; } = Default[1];

        public static string IRC_Chan { get; set; } = Default[2];

        public static string IRC_Nick { get; set; } = Default[3];

        public static string IRC_User { get; set; } = Default[3];

        public static string IRC_Pass { get; set; } = Default[4];


        public static string[,] Commands { get; set; } = {
            //{ "stats", "Stats for this channel can be found at https://chanstats.snoonet.org/%23eldorito.html" },
            { "Orion", "OH-RI-ON" },
            { "clef", "Nice IRC'r" }
        };

        public static string[] Triggers { get; set; } = {
            "theTwister",
            "Orion"
        };

        public static string[,] Users { get; set; } = {
            { "theTwister", "Thank you based programmer for my existence!" },
            { "Shockfire", "theTwister made me a Synth!" },
            { "dany", "Sup dany... Hows the current progress on [insert latest project here]!" },
            { "Camden", "Ayy!" },
            { "DEElekgolo", "A wild DEElekgolo appeared!"},
            { "emoose", "Our lord and savior has returned!"},
            { "Alex231;Alex-231", "HUD master!"},
            { "RabidSquabbit", "Host master!"},
            { "Divide;G-Money;Weion", "Host disciple!"}
        };


        public static string Dir { get; set; } = Directory.GetCurrentDirectory();

        public static string Date = DateTime.Today.ToShortDateString();

        public static string Time = DateTime.Now.ToShortTimeString();
    }
}