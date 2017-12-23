using System;
using System.Collections;
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

        public static void ExceptionLog(string Message = "", string Filename = "", string SourcePage = "boredsilly.in")
        {
            try
            {
                Hashtable browserInfo = GetBrowserInfo();
                if (string.IsNullOrEmpty(Filename.Trim())) Filename = _logFilename + ".txt";
                else Filename += "_" + _logFilename + ".txt";
                if (string.IsNullOrEmpty(Message.Trim()))
                    Message = browserInfo["UserAgent"].ToString();

                SourcePage = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";
                string browserData = browserInfo["Browser"].ToString() + " " + browserInfo["BrowserVersion"].ToString() + " " + browserInfo["MobileDevice"].ToString();

                using (StreamWriter _writer = File.AppendText(_logPath + "\\" + Filename))
                {
                    LogWriter(Message, _writer, SourcePage, browserData);
                }
            }
            catch (Exception ex)
            {
            }
        }
        private static void LogWriter(string logMessage, TextWriter txtWriter, string SourcePage = "http://boredsilly.in/", string browserData ="")
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0}", Utils.FullDateTimeFormatIST());
                txtWriter.WriteLine("Source : {0} , Ip: {1}", SourcePage, Utils.GetIPAddress());
                txtWriter.WriteLine("System : " + browserData);
                txtWriter.WriteLine("Message : {0}", logMessage);
                txtWriter.WriteLine("------------------------------------------------");
            }
            catch (Exception ex) { }
        }
        public static Hashtable GetSystemInfo()
        {
            try
            {
                Hashtable _SystemInfo = new Hashtable();
                _SystemInfo.Add("UserDomainName", System.Environment.UserDomainName);
                _SystemInfo.Add("MachineName", System.Environment.MachineName);
                _SystemInfo.Add("UserName", System.Environment.UserName);
                _SystemInfo.Add("OSVersion", System.Environment.OSVersion.ToString());
                _SystemInfo.Add("OSVersionNo", System.Environment.Version.ToString());
                _SystemInfo.Add("SystemDirectory", System.Environment.SystemDirectory);
                _SystemInfo.Add("CommandLine", System.Environment.CommandLine);
                _SystemInfo.Add("CurrentDirectory", System.Environment.CurrentDirectory);
                _SystemInfo.Add("CurrentManagedThreadId", System.Environment.CurrentManagedThreadId.ToString());
                _SystemInfo.Add("ExitCode", System.Environment.ExitCode.ToString());
                _SystemInfo.Add("Is64BitOperatingSystem", System.Environment.Is64BitOperatingSystem.ToString());
                _SystemInfo.Add("Is64BitProcess", System.Environment.Is64BitProcess.ToString());
                _SystemInfo.Add("NewLine", System.Environment.NewLine.ToString());
                _SystemInfo.Add("ProcessorCount", System.Environment.ProcessorCount.ToString());
                _SystemInfo.Add("StackTrace", System.Environment.StackTrace.ToString());
                _SystemInfo.Add("SystemPageSize", System.Environment.SystemPageSize.ToString());
                _SystemInfo.Add("TickCount", System.Environment.TickCount.ToString());
                _SystemInfo.Add("UserInteractive", System.Environment.UserInteractive.ToString());
                _SystemInfo.Add("WorkingSet", System.Environment.WorkingSet.ToString());

                return _SystemInfo;
            }
            catch (Exception ex)
            {
                Common.ExceptionLog(ex.Message);
                throw ex;
            }
        }
        public static Hashtable GetBrowserInfo()
        {
            Hashtable _BrowserInfo = new Hashtable ();
            try
            {
                _BrowserInfo.Add("Browser", HttpContext.Current.Request.Browser.Browser);
                _BrowserInfo.Add("BrowserVersion", HttpContext.Current.Request.Browser.Version);
                _BrowserInfo.Add("ScreenPixelsWidth", HttpContext.Current.Request.Browser.ScreenPixelsWidth);
                _BrowserInfo.Add("SupportsXmlHttp", HttpContext.Current.Request.Browser.SupportsXmlHttp);
                _BrowserInfo.Add("Platform", HttpContext.Current.Request.Browser.Platform);

                string userAgent = HttpContext.Current.Request.UserAgent.ToLower();
                
                string _mobileDevice = string.Empty;
                if (HttpContext.Current.Request.Browser.IsMobileDevice)
                {
                    string[] _mobileDevices = new string[] { "iphone", "android", "ppc", "windows ce", "blackberry", "opera mini", "mobile", "palm", "portable", "opera mobi" };
                    foreach (var mobDevice in _mobileDevices)
                    {
                        if (userAgent.Contains(mobDevice))
                        {
                            _mobileDevice = Utils.ConvertToTitleString(mobDevice);
                            break;
                        }
                    }
                    _mobileDevice += " " + HttpContext.Current.Request.Browser.MobileDeviceModel;
                }

                _BrowserInfo.Add("IsMobileDevice", HttpContext.Current.Request.Browser.IsMobileDevice);
                _BrowserInfo.Add("MobileDevice", _mobileDevice);
                _BrowserInfo.Add("MobileDeviceManufacturer", HttpContext.Current.Request.Browser.MobileDeviceManufacturer);
                _BrowserInfo.Add("MobileDeviceModel", HttpContext.Current.Request.Browser.MobileDeviceModel);
                _BrowserInfo.Add("UserAgent", userAgent);
            }
            catch (Exception ex)
            {
                Common.ExceptionLog(ex.Message);
                throw ex;
            }
            return _BrowserInfo;
        }
               

    }
}
