using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BinkodApp.Core
{
    public class Common
    {
        private static string _logPath = HttpContext.Current.Server.MapPath("~/Content/Logs");
        private static string _logFilename = "Log_" + Utils.DateFormatForFilename();
        public static void ExceptionLog(string Message = "", string Filename= "", string SourcePage = "boredsilly.in")
        {
            try
            {
                if (string.IsNullOrEmpty(Filename))
                    Filename = _logFilename + ".txt";
                else
                    Filename += "_" + _logFilename + ".txt";
                
                using (StreamWriter _writer = File.AppendText(_logPath + "\\" + Filename))
                {
                    LogWriter(Message, _writer, SourcePage);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private static void LogWriter(string logMessage, TextWriter txtWriter, string SourcePage)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", Utils.DateFormatIST(), Utils.TimeFormatIST());
                txtWriter.WriteLine("Source : {0}", "http://boredsilly.in/");
                txtWriter.WriteLine("Message : {0}", logMessage);
                txtWriter.WriteLine("------------------------------------------------");
            }
            catch (Exception ex)
            {
            }
        }
    }
}
