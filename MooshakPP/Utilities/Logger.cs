using System;
using System.Configuration;
using System.IO;
using System.Web;

namespace MooshakPP.Utilities
{
    public class Logger
    {
        private static Logger theInstance = null;
        public static Logger Instance
        {
            get
            {
                if (theInstance == null)
                {
                    theInstance = new Logger();
                }
                return theInstance;
            }
        }

        public void LogException(Exception ex)
        {
            // Directory containing logfile
            string logFileDir = ConfigurationManager.AppSettings["LogFileDir"];
            logFileDir = HttpContext.Current.Server.MapPath(logFileDir);
            string logFile = logFileDir + "Error log.txt";

            string message = string.Format("{0} was thrown on the {1}.{4}For: {2}{3}{4}",
                ex.Message, DateTime.Now, ex.Source, ex.StackTrace, Environment.NewLine);

            if (!Directory.Exists(logFile))
            {
                Directory.CreateDirectory(logFileDir);
            }
            using (StreamWriter writer = new StreamWriter(logFile, true))
            {
                writer.WriteLine(message);
            }

        }
    }
}