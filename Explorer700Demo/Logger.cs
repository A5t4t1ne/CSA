using System;
using System.IO;
using System.Text;

namespace Explorer700Demo
{
    public class Logger
    {
        private static Logger instance = new Logger();
        private string logFile = "./log.txt";

        private Logger() {}

        public static Logger Instance
        {
            get { return instance; }
        }

        public void LogEntry(string message)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(logFile, true, Encoding.UTF8))
                {
                    if (new FileInfo(logFile).Length == 0)
                    {
                        sw.WriteLine("// Logs from Application - Team 08");
                    }
                    sw.WriteLine(message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
