using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BPCloud.MailReminder.Service
{
    public class ReminderMailSender
    {
        public static string BaseAddress = ConfigurationManager.AppSettings["BaseAddress"];
        public static void StartReminder()
        {
            SendReminderToInitializedVendor();
            SendReminderToSavedVendor();
            SendReminderToRegisteredVendor();
            //ConvertXml();
        }

        public static bool SendReminderToInitializedVendor()
        {
            bool ReminderToInitializedVendor = false;
            try
            {
                ErrorLog.WriteLog("Calling Registration Controller in SendReminderToInitializedVendor method");
                var url = BaseAddress + "vendorregisterapi/Registration/SendReminderToInitializedVendor";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        ReminderToInitializedVendor = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ReminderToInitializedVendor = false;
                ErrorLog.WriteLog("SendReminderToInitializedVendor : " + ex.Message);
            }
            return ReminderToInitializedVendor;
        }

        public static bool SendReminderToSavedVendor()
        {
            bool ReminderToSavedVendor = false;
            try
            {
                ErrorLog.WriteLog("Calling Registration Controller in SendReminderToSavedVendor method");
                var url = BaseAddress + "vendorregisterapi/Registration/SendReminderToSavedVendor";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        ReminderToSavedVendor = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ReminderToSavedVendor = false;
                ErrorLog.WriteLog("SendReminderToSavedVendor : " + ex.Message);
            }
            return ReminderToSavedVendor;
        }

        public static bool SendReminderToRegisteredVendor()
        {
            bool ReminderToRegisteredVendor = false;
            try
            {
                ErrorLog.WriteLog("Calling Registration Controller in SendReminderToRegisteredVendor method");
                var url = BaseAddress + "vendorregisterapi/Registration/SendReminderToRegisteredVendor";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        ReminderToRegisteredVendor = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ReminderToRegisteredVendor = false;
                ErrorLog.WriteLog("SendReminderToRegisteredVendor : " + ex.Message);
            }
            return ReminderToRegisteredVendor;
        }
    }
}
