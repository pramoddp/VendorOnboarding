﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPCloud.MailReminder
{
    public static class ErrorLog
    {
        public static void WriteLog(string Message, Exception ex = null)
        {
            StreamWriter sw = null;
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogFiles");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                DateTime dt = DateTime.Today;
                DateTime ystrdy = DateTime.Today.AddDays(-15);//keep 15 days backup
                string yday = ystrdy.ToString("yyyyMMdd");
                string today = dt.ToString("yyyyMMdd");
                string Log = today + ".txt";
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\LogFiles\\Log_" + yday + ".txt"))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\LogFiles\\Log_" + yday + ".txt");
                }
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFiles\\Log_" + Log, true);
                sw.WriteLine(string.Format(DateTime.Now.ToString()) + ":" + Message);
                WriteInnerExceptionDetails(sw, ex);
                sw.Flush();
                sw.Close();
            }
            catch
            {

            }

        }
        public static void WriteInnerExceptionDetails(StreamWriter sw, Exception ex)
        {
            if (ex != null && ex.Message.Contains("inner exception") && ex.InnerException != null)
            {
                sw.WriteLine($"{DateTime.Now.ToString()} : Inner :- {ex.InnerException.Message}");
                WriteInnerExceptionDetails(sw, ex.InnerException);
            }
            else
            {
                return;
            }
        }
    }
}
