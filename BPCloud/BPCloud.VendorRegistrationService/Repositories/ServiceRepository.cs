using BPCloud.VendorRegistrationService.DBContexts;
using BPCloud.VendorRegistrationService.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BPCloud.VendorRegistrationService.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly RegistrationContext _dbContext;
        IConfiguration _configuration;
        public ServiceRepository(RegistrationContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public async Task SendReminderToInitializedVendor()
        {
            try
            {
                var Today = DateTime.Now;
                string PortalAddress = _configuration["PortalAddress"];
                var InitializedVendor = _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "initialized").ToList();
                foreach (var vendor in InitializedVendor)
                {
                    var tokenhistory = _dbContext.TokenHistories.Where(x => x.TransID == vendor.TransID).FirstOrDefault();
                    if (tokenhistory != null)
                    {
                        if (tokenhistory.ExpireOn >= Today)
                        {
                            var tokenTimespan = (tokenhistory.ExpireOn - Today).TotalDays;
                            bool sendresult = await SendMail(HttpUtility.UrlEncode(tokenhistory.Token), vendor.Name, vendor.Email1, vendor.TransID.ToString(), PortalAddress, "Vendor Registration - Reminder", tokenTimespan.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task SendReminderToSavedVendor()
        {
            try
            {
                var Today = DateTime.Now;
                string PortalAddress = _configuration["PortalAddress"];
                var SavedVendor = _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "saved").ToList();
                foreach (var vendor in SavedVendor)
                {
                    var tokenhistory = _dbContext.TokenHistories.Where(x => x.TransID == vendor.TransID).FirstOrDefault();
                    if (tokenhistory != null)
                    {
                        if (tokenhistory.ExpireOn >= Today)
                        {
                            var tokenTimespan = (tokenhistory.ExpireOn - Today).TotalDays;
                            bool sendresult = await SendMail(HttpUtility.UrlEncode(tokenhistory.Token), vendor.Name, vendor.Email1, vendor.TransID.ToString(), PortalAddress, "Vendor Registration - Reminder", tokenTimespan.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task SendReminderToRegisteredVendor()
        {
            try
            {
                var Today = DateTime.Now;
                string PortalAddress = _configuration["PortalAddress"];
                string AuthenticationAPIAddress = _configuration["AuthenticationAPIAddress"];
                var admins = new List<User>();
                var RegisteredVendor = _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "registered").ToList();
                try
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(AuthenticationAPIAddress + "api/Master/GetApprovers");
                    request.Method = "GET";
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        Stream dataStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(dataStream);
                        string jsonRes = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        admins = JsonConvert.DeserializeObject<List<User>>(jsonRes);
                        reader.Close();
                        dataStream.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                if (admins.Count > 0)
                {
                    foreach (var vendor in RegisteredVendor)
                    {
                        foreach(var admin in admins)
                        {
                            bool sendresult = await SendMailToApprover(admin.UserName, admin.Email, vendor.TransID.ToString(), PortalAddress, "Vendor Approval - Reminder");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> SendMail(string code, string UserName, string toEmail, string TransID, string siteURL, string Subject, string tokenTimespan)
        {
            try
            {
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];
                var message = new MailMessage();
                StringBuilder sb = new StringBuilder();
                //string UserName = _ctx.TBL_User_Master.Where(x => x.Email == toEmail).Select(y => y.UserName).FirstOrDefault();
                //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
                //sb.Append(string.Format("Dear {0},<br/>", UserName));
                //sb.Append("You have invited to register in our business process by Wipro, Request you to proceed with registration");
                //sb.Append("<p><a href=\"" + siteURL + "/#/register/vendor?token=" + code + "&Id=" + TransID + "&Email=" + toEmail + "\"" + "></a>Register</p>");
                //sb.Append($"<i>Note: The verification link will expire in {tokenTimespan} days.<i>");
                //sb.Append("<p>Regards,</p><p>Admin</p>");
                sb.Append(@"<html><head></head><body> <div style='border:1px solid #dbdbdb;'> <div style='padding: 20px 20px; background-color: #fff06769;text-align: center;font-family: Segoe UI;'> <p> <h2>Wipro Vendor Onboadring</h2> </p> </div> <div style='background-color: #f8f7f7;padding: 20px 20px;font-family: Segoe UI'> <div style='padding: 20px 20px;border:1px solid white;background-color: white !important'> <p>Dear concern,</p> <p>You have invited to register in our business process by Wipro, Request you to proceed with registration.</p> <div style='text-align: end;'>" + "<a href=\"" + siteURL + "/#/register/vendor?token=" + code + "&Id=" + TransID + "&Email=" + toEmail + "\"" + "><button style='width: 90px;height: 28px; background-color: #039be5;color: white'>Register</button> </div> <p>Note: The verification link will expire in " + tokenTimespan + " days.</p> <p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>");
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = true;
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);
                MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, Subject, sb.ToString());
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                //ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                await client.SendMailAsync(reportEmail);
                WriteLog.WriteToFile($"Registration Reminder link has been sent successfully to {toEmail}");
                return true;
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        WriteLog.WriteToFile("ServiceRepository/SendMail/MailboxBusy/MailboxUnavailable/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                    else
                    {
                        WriteLog.WriteToFile("ServiceRepository/SendMail/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                }
                WriteLog.WriteToFile("ServiceRepository/SendMail/SmtpFailedRecipientsException:- " + ex.Message, ex);
                return false;
            }
            catch (SmtpException ex)
            {
                WriteLog.WriteToFile("ServiceRepository/SendMail/SmtpException:- " + ex.Message, ex);
                return false;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ServiceRepository/SendMail/Exception:- " + ex.Message, ex);
                return false;
            }
        }

        public async Task<bool> SendMailToApprover(string UserName, string toEmail, string TransID, string siteURL, string Subject)
        {
            try
            {
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];
                var message = new MailMessage();
                StringBuilder sb = new StringBuilder();
                //string UserName = _ctx.TBL_User_Master.Where(x => x.Email == toEmail).Select(y => y.UserName).FirstOrDefault();
                //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
                //sb.Append(string.Format("Dear {0},<br/>", UserName));
                //sb.Append($"The registered vendor with {TransID} need to be approved.Please take an action by login the business process portal");
                //sb.Append("<p>Please Login by clicking <a href=\"" + siteURL + "/#/auth/login\">here</a></p>");
                //sb.Append("<p>Regards,</p><p>Admin</p>");
                sb.Append(@"<html><head></head><body> <div style='border:1px solid #dbdbdb;'> <div style='padding: 20px 20px; background-color: #fff06769;text-align: center;font-family: Segoe UI;'> <p> <h2>Wipro Vendor Onboadring</h2> </p> </div> <div style='background-color: #f8f7f7;padding: 20px 20px;font-family: Segoe UI'> <div style='padding: 20px 20px;border:1px solid white;background-color: white !important'> <p>Dear concern,</p> <p>The registered vendor with " + TransID + " need to be approved.Please take an action by login the business process portal</p> <div style='text-align: end;'>" + "<a href=\"" + siteURL + "/#/auth/login\">,</a><button style='width: 90px;height: 28px; background-color: #039be5;color: white'>Approve</button> </div> <p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>");
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = true;
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);
                MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, Subject, sb.ToString());
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                //ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                await client.SendMailAsync(reportEmail);
                WriteLog.WriteToFile($"Approval link has been sent successfully to {toEmail}");
                return true;
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        WriteLog.WriteToFile("ServiceRepository/SendMailToApprover/MailboxBusy/MailboxUnavailable/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                    else
                    {
                        WriteLog.WriteToFile("ServiceRepository/SendMailToApprover/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                }
                WriteLog.WriteToFile("ServiceRepository/SendMailToApprover/SmtpFailedRecipientsException:- " + ex.Message, ex);
                return false;
            }
            catch (SmtpException ex)
            {
                WriteLog.WriteToFile("ServiceRepository/SendMailToApprover/SmtpException:- " + ex.Message, ex);
                return false;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ServiceRepository/SendMailToApprover/Exception:- " + ex.Message, ex);
                return false;
            }
        }

    }
}
