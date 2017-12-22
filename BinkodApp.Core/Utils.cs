using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BinkodApp.Core
{
    public class Utils
    {
        public static String GetIPAddress()
        {
            try
            {            
                String _ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(_ipAddress))
                    _ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                else
                    _ipAddress = _ipAddress.Split(',')[0];

                if (_ipAddress == "::1")
                    _ipAddress = "127.0.0.1";

                return _ipAddress;
            }
            catch (Exception ex)
            {
                Dictionary<string, string> _ServerVariables = new Dictionary<string, string>();
                foreach (var variable in HttpContext.Current.Request.ServerVariables.AllKeys)
                {
                    _ServerVariables.Add(variable.ToString(), HttpContext.Current.Request.ServerVariables[variable.ToString()]);
                }
                throw ex;
            }
        }

        public static Dictionary<string, string> GetSystemInfo()
        {
            try
            {
                Dictionary<string, string> _SystemInfo = new Dictionary<string, string>();
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
                throw ex;
            }
        }

        public static string DataTableToCSV(System.Data.DataTable table)
        {
            var result = new System.Text.StringBuilder();
            for (int i = 0; i < table.Columns.Count; i++)
            {
                result.Append(table.Columns[i].ColumnName);
                result.Append(i == table.Columns.Count - 1 ? "\n" : ",");
            }
            string value = string.Empty;
            foreach (System.Data.DataRow row in table.Rows)
            {

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    value = string.Empty;
                    value = row[i].ToString();
                    // if (value.IndexOfAny(new char[] { '"', ',' }) != -1)
                    value = string.Format("\"{0}\"", value.Replace("\"", "\"\""));

                    result.Append(value);
                    result.Append(i == table.Columns.Count - 1 ? "\n" : ",");
                }
            }

            return result.ToString();
        }

        public static string DataTableToJSONWithStringBuilder(System.Data.DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }

        public static string NumberToWords(Int64 number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000000000000) > 0)
            {
                words += NumberToWords(number / 1000000000000000) + " quadrillion ";
                number %= 1000000000000000;
            }

            if ((number / 1000000000000) > 0)
            {
                words += NumberToWords(number / 1000000000000) + " trillion ";
                number %= 1000000000000;
            }

            if ((number / 1000000000) > 0)
            {
                words += NumberToWords(number / 1000000000) + " billion ";
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        public static string DecimalToWords(Decimal number, int multiplier = 100)
        {
            long fractionalPart = (long)((number - (long)number) * multiplier);

            string words = "";

            words = NumberToWords(Convert.ToInt64(Math.Floor(number)));

            if (fractionalPart > 0)
            {
                words += " point ";

                string strFractionalPart = string.Empty;
                if (number.ToString().IndexOf('.') > 0)
                {
                    strFractionalPart = number.ToString().Substring(number.ToString().IndexOf('.') + 1);

                    foreach (char character in strFractionalPart)
                    {
                        if (character != '0')
                            break;

                        words += "zero ";
                    }
                }

                words += NumberToWords(Convert.ToInt64(fractionalPart));
            }
            words = words.Trim();
            return words;
        }

        public static String DateTimeFormatForFilename()
        {
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi); // convert from utc to local
            return localTime.ToString("dd_MM_yyyy_HHmmss");
        }
        public static String DateFormatForFilename()
        {
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi); // convert from utc to local
            return localTime.ToString("dd_MM_yyyy");
        }

        public static String DateTimeFormatIST()
        {
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi); // convert from utc to local
            return localTime.ToString();
        }
        public static String DateFormatIST()
        {
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi); // convert from utc to local
            return localTime.ToLongDateString();
        }
        public static String TimeFormatIST()
        {
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi); // convert from utc to local
            return localTime.ToLongTimeString();
        }
        public static String ShortDateFormatIST()
        {
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi); // convert from utc to local
            return localTime.ToShortDateString();
        }
        public static String ShortTimeFormatIST()
        {
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi); // convert from utc to local
            return localTime.ToShortTimeString();
        }

        public static void DeleteFiles_ThreeMonthOlder()
        {
            try
            {
                string ScreenshotPath = HttpContext.Current.Server.MapPath("~/Images/Screenshots");
                string[] files = Directory.GetFiles(ScreenshotPath);

                foreach (string file in files)
                {
                    FileInfo _fileInfo = new FileInfo(file);
                    if (_fileInfo.LastAccessTime < DateTime.Now.AddMonths(-3))
                        _fileInfo.Delete();
                }
            }
            catch (Exception ex) { }
        }

        #region  Encode/Decode into Base36

        private const string CharList = "0123456789abcdefghijklmnopqrstuvwxyz";        
        //Encode the given number into a Base36 string
        public static String EncodeIntoBase36(long input)
        {
            if (input < 0) throw new ArgumentOutOfRangeException("input", input, "input cannot be negative");

            char[] clistarr = CharList.ToCharArray();
            var result = new Stack<char>();
            while (input != 0)
            {
                result.Push(clistarr[input % 36]);
                input /= 36;
            }
            return new string(result.ToArray());
        }
        
        //Decode the Base36 Encoded string into a number
        public static Int64 DecodeIntoBase36(string input)
        {
            var reversed = input.ToLower().Reverse();
            long result = 0;
            int pos = 0;
            foreach (char c in reversed)
            {
                result += CharList.IndexOf(c) * (long)Math.Pow(36, pos);
                pos++;
            }
            return result;
        }
        #endregion
    }
}
