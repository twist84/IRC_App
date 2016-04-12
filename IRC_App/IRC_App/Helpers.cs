using System;
using System.IO;
using System.Threading.Tasks;

namespace IRC_Lib
{
    public class Timeout
    {
        public async static void Start()
        {
            await TimeOut_Method(Int32.Parse(Settings.GetSetting("TimeOut")));
        }

        public static async Task<int> TimeOut_Method(int TimeOut)
        {
            Settings.IsTimedOut = true;
            for (int i = TimeOut * 1; i > 0; i--)
            {
                await Task.Delay(1000);
                //Console.WriteLine("{0} Seconds Left", i);
            }
            Settings.IsTimedOut = false;
            return 1;
        }
    }

    public class Logger
    {
        public static void Log(string Message)
        {
            if (Settings.IsLogging.Equals(true))
            {
                string L_Log = String.Format("{0}\\Log_{1}.txt", Settings.Dir, Settings.Date);
                string L_Url = String.Format("{0}\\Url_{1}.txt", Settings.Dir, Settings.Date);
                string L_Urll = String.Format("{0}\\Url_{1}.txt at Timestamp {1}", Settings.Dir, Settings.Time);

                if (Message.Contains("ftp://") || Message.Contains("http://") || Message.Contains("https://"))
                {
                    File.AppendAllText(L_Url, Message + Environment.NewLine);
                    File.AppendAllText(L_Log, L_Urll + Environment.NewLine);
                }
                else
                    File.AppendAllText(L_Log, Message + Environment.NewLine);

                Console.WriteLine("[{0}] Now talking on {1}", Settings.Date, Settings.Time);
            }
        }
    }

    public class DEBUG
    {
        public static string Message;

        public static void SHOW(params string[] Params)
        {
            Message = null;

            if (Settings.IsDEBUG.Equals(true))
            {
                if (Params.Length > 1)
                {
                    for (int i = 1; i < Params.Length; i++)
                    {
                        Message = Params[i];
                        Console.WriteLine(Message);
                    }
                }
                else
                {
                    Message = Params[0];
                    Console.WriteLine(Message);
                }
            }
        }
    }
}
