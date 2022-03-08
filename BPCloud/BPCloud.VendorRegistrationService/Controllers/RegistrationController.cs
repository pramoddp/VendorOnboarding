using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BPCloud.VendorRegistrationService.Models;
using BPCloud.VendorRegistrationService.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace BPCloud.VendorRegistrationService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IVendorOnBoardingRepository _VendorOnBoardingRepository;
        private readonly IIdentityRepository _IdentityRepository;
        private readonly IBankRepository _BankRepository;
        private readonly IContactRepository _ContactRepository;
        private readonly IActivityLogRepository _ActivityLogRepository;
        private readonly ITextRepository _TextRepository;
        private readonly IQuestionnaireRepository _QuestionnaireRepository;
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IServiceRepository _ServiceRepository;
        private IConfiguration _configuration;

        public RegistrationController(IVendorOnBoardingRepository vendorOnBoardingRepository, IIdentityRepository identityRepository,
            IBankRepository bankRepository, IContactRepository contactRepository, IActivityLogRepository activityLogRepository,
            ITextRepository textRepository, IServiceRepository ServiceRepository, IConfiguration configuration)
        {
            _VendorOnBoardingRepository = vendorOnBoardingRepository;
            _IdentityRepository = identityRepository;
            _BankRepository = bankRepository;
            _ContactRepository = contactRepository;
            _ActivityLogRepository = activityLogRepository;
            _TextRepository = textRepository;
            _ServiceRepository = ServiceRepository;
            _configuration = configuration;
        }

        #region BPVendorOnBoarding

        [HttpGet]
        public List<BPVendorOnBoarding> GetAllVendorOnBoardings()
        {
            try
            {
                var VendorOnBoardings = _VendorOnBoardingRepository.GetAllVendorOnBoardings();
                return VendorOnBoardings;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetAllVendorOnBoardings", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPVendorOnBoarding> GetAllOpenVendorOnBoardings()
        {
            try
            {
                var VendorOnBoardings = _VendorOnBoardingRepository.GetAllOpenVendorOnBoardings();
                return VendorOnBoardings;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetAllOpenVendorOnBoardings", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPVendorOnBoarding> GetAllApprovedVendorOnBoardings()
        {
            try
            {
                var VendorOnBoardings = _VendorOnBoardingRepository.GetAllApprovedVendorOnBoardings();
                return VendorOnBoardings;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetAllApprovedVendorOnBoardings", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPVendorOnBoarding> GetAllRejectedVendorOnBoardings()
        {
            try
            {
                var VendorOnBoardings = _VendorOnBoardingRepository.GetAllRejectedVendorOnBoardings();
                return VendorOnBoardings;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetAllRejectedVendorOnBoardings", ex);
                return null;
            }
        }

        [HttpPost]
        public List<BPVendorOnBoarding> GetAllOpenVendorOnBoardingsByPlant([FromBody]List<string> Plants)
        {
            try
            {
                var VendorOnBoardings = _VendorOnBoardingRepository.GetAllOpenVendorOnBoardingsByPlant(Plants);
                return VendorOnBoardings;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetAllOpenVendorOnBoardingsByPlant", ex);
                return null;
            }
        }
        [HttpGet]
        public List<BPVendorOnBoarding> GetAllOpenVendorOnBoardingsByApprover(string Approver)
        {
            try
            {
                var VendorOnBoardings = _VendorOnBoardingRepository.GetAllOpenVendorOnBoardingsByApprover(Approver);
                return VendorOnBoardings;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetAllOpenVendorOnBoardingsByApprover", ex);
                return null;
            }
        }
        [HttpGet]
        public List<BPVendorOnBoarding> GetAllApprovedVendorOnBoardingsByApprover(string Approver)
        {
            try
            {
                var VendorOnBoardings = _VendorOnBoardingRepository.GetAllApprovedVendorOnBoardingsByApprover(Approver);
                return VendorOnBoardings;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetAllApprovedVendorOnBoardingsByApprover", ex);
                return null;
            }
        }
        [HttpPost]
        public List<BPVendorOnBoarding> GetAllApprovedVendorOnBoardingsByPlant([FromBody] List<string> Plants)
        {
            try
            {
                var VendorOnBoardings = _VendorOnBoardingRepository.GetAllApprovedVendorOnBoardingsByPlant(Plants);
                return VendorOnBoardings;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetAllApprovedVendorOnBoardingsByPlant", ex);
                return null;
            }
        }
        [HttpPost]
        public List<BPVendorOnBoarding> GetAllRejectedVendorOnBoardingsByPlant([FromBody] List<string> Plants)
        {
            try
            {
                var VendorOnBoardings = _VendorOnBoardingRepository.GetAllRejectedVendorOnBoardingsByPlant(Plants);
                return VendorOnBoardings;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetAllRejectedVendorOnBoardingsByPlant", ex);
                return null;
            }
        }
        [HttpGet]
        public List<BPVendorOnBoarding> GetAllRejectedVendorOnBoardingsByApprover(string Approver)
        {
            try
            {
                var VendorOnBoardings = _VendorOnBoardingRepository.GetAllRejectedVendorOnBoardingsByApprover(Approver);
                return VendorOnBoardings;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetAllRejectedVendorOnBoardingsByApprover", ex);
                return null;
            }
        }

        #region identity
        [HttpGet]
        public List<BPIdentity> GetAllIdentity(int TransID)
        {
            try
            {
                var BPIdentity = _VendorOnBoardingRepository.GetAllIdentity(TransID);
                return BPIdentity;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetAllIdentity", ex);
                return null;
            }
        }

        #region Declaration ID
        [HttpGet]
        public BPVendorOnBoarding[] GetDeclarationID(int TransID)
        {
            try
            {
                var DeclarationID = _VendorOnBoardingRepository.GetDeclarationID(TransID);
                string[] array1 = new string[3];
                array1[0] = DeclarationID[0].MSME_Att_ID;
                array1[1] = DeclarationID[0].RP_Att_ID;
                array1[2] = DeclarationID[0].TDS_Att_ID;
                BPVendorOnBoarding[] result = new BPVendorOnBoarding[3];
                for (int i = 0; i < 3; i++)
                {

                    result[i] = _VendorOnBoardingRepository.GetAttachmentId(array1[i]);
                };
                //var DeclarationID2 =
                return result;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetDeclarationID", ex);
                return null;
            }

        }
        #endregion

        #region toogle
        [HttpGet]
        public List<BPVendorOnBoarding> GetDeclaration_toogle(int TransID)
        {
            var result = _VendorOnBoardingRepository.GetDeclaration_toogle(TransID);
            return result;
        }
        #endregion

        #region attachmentFile
        //public BPAttachment GetAttachmentFile(int TransID, string attachmentname)
        public List<BPAttachment> GetAttachmentFile(int TransID, string attachmentname)

        {
            try
            {
                var result1 = _VendorOnBoardingRepository.GetAttachmentFile(TransID, attachmentname);
                //if()
                return result1;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetAttachmentFile", ex);
                return null;
            }
        }
        #endregion
        #endregion


        [HttpGet]
        public IActionResult GetAllOpenVendorOnBoardingsCount()
        {
            try
            {
                var VendorOnBoardings = _VendorOnBoardingRepository.GetAllOpenVendorOnBoardingsCount();
                return Ok(VendorOnBoardings.Count());
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetAllOpenVendorOnBoardings", ex);
                return null;
            }
        }

        [HttpGet]
        public IActionResult GetAllOpenVendorOnBoardingsCountByApprover(string Approver)
        {
            try
            {
                var VendorOnBoardings = _VendorOnBoardingRepository.GetAllOpenVendorOnBoardingsCountByApprover(Approver);
                return Ok(VendorOnBoardings.Count());
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetAllOpenVendorOnBoardings", ex);
                return null;
            }
        }

        [HttpGet]
        public IActionResult GetAllApprovedVendorOnBoardingsCount()
        {
            try
            {
                var VendorOnBoardings = _VendorOnBoardingRepository.GetAllApprovedVendorOnBoardingsCount();
                return Ok(VendorOnBoardings.Count());
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetAllApprovedVendorOnBoardingsCount", ex);
                return null;
            }
        }

        [HttpGet]
        public IActionResult GetAllRejectedVendorOnBoardingsCount()
        {
            try
            {
                var VendorOnBoardings = _VendorOnBoardingRepository.GetAllRejectedVendorOnBoardingsCount();
                return Ok(VendorOnBoardings.Count());
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetAllRejectedVendorOnBoardingsCount", ex);
                return null;
            }
        }

        [HttpGet]
        public BPVendorOnBoarding GetVendorOnBoardingsByID(int TransID)
        {
            try
            {
                var VendorOnBoardings = _VendorOnBoardingRepository.GetVendorOnBoardingsByID(TransID);
                return VendorOnBoardings;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetVendorOnBoardingsByID", ex);
                return null;
            }
        }

        [HttpGet]
        public BPVendorOnBoarding GetVendorOnBoardingsByEmailID(string EmailID)
        {
            try
            {
                var VendorOnBoardings = _VendorOnBoardingRepository.GetVendorOnBoardingsByEmailID(EmailID);
                return VendorOnBoardings;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetVendorOnBoardingsByEmailID", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPVendorOnBoarding> GetRegisteredVendorOnBoardings()
        {
            try
            {
                var VendorOnBoardings = _VendorOnBoardingRepository.GetRegisteredVendorOnBoardings();
                return VendorOnBoardings;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetAllVendorOnBoardings", ex);
                return null;
            }
        }


        //[HttpPost]
        //public async Task<IActionResult> CreateVendorOnBoarding(BPVendorOnBoarding VendorOnBoarding)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }
        //        await _VendorOnBoardingRepository.CreateVendorOnBoarding(VendorOnBoarding);
        //        return CreatedAtAction(nameof(GetAllVendorOnBoardings), new { TransID = VendorOnBoarding.TransID }, VendorOnBoarding);
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.WriteToFile("Registration/CreateVendorOnBoarding", ex);
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> InitializeVendorRegistration(VendorInitialzationClass vendorInitialzationClass)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _VendorOnBoardingRepository.InitializeVendorRegistration(vendorInitialzationClass);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/InitializeVendorRegistration", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult ChectTokenValidity(VendorTokenCheck tokenCheck)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _VendorOnBoardingRepository.ChectTokenValidity(tokenCheck);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/InitializeVendorRegistration", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> sendmail()
        {
            var UserName = "Star";
            var toEmail = "stars8784@gmail.com";
            var siteURL = "";
            var code = "";
            var TransID = "";
            try
            {
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];

                //var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                //var logoimage = Path.Combine(outPutDirectory, "Images\\Emami.png");
                //string relLogo = new Uri(logoimage).LocalPath;
                //var logoImage = new LinkedResource(relLogo);

                var dir = Directory.GetCurrentDirectory() + "\\Images\\Emami.png";
                //var imageAsBase64 = Convert.ToBase64String(File.ReadAllBytes(image));

                var message = new MailMessage();
                string subject = "";
                StringBuilder sb = new StringBuilder();
                //string UserName = _ctx.TBL_User_Master.Where(x => x.Email == toEmail).Select(y => y.UserName).FirstOrDefault();
                //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
                //sb.Append(string.Format("Dear {0},<br/>", UserName));
                //sb.Append("You have invited to register in our BPCloud by Emami Limited, Request you to proceed with registration");
                //sb.Append("<p>cccc</p>");
                //sb.Append($"<i>Note: The verification link will expire in {30} days.<i>");
                //sb.Append("<p>Regards,</p><p>Admin</p>");


                //sb.Append(@"<html><head></head><body><div style='border:1px solid green;'><div style='padding: 20px 20px; background-color: #fff06769;'><p>Hi Balamanikandan Subramaniam,</p><p>Almost the end of the week! </p></div><div style='background-color: #f8f7f7;padding: 20px 20px;'><div style='padding: 20px 20px;border:1px solid white;background-color: white !important;'><p>Dear concern</p><p>Vendor Registration Link Initiliazed</p><div style='text-align: end;'><button style='width: 90px;height: 28px;color: black;display: inline-block;font-size: 16px;margin: 4px 2px;transition-duration: 0.4s;cursor: pointer;letter-spacing: 0.3px;border: 1px solid #008CBA;font-family: 'Segoe UI', Tahoma,Geneva, Verdana, sans-serif;border-radius: 3px;background-color: white ;'>Register</button></div></div></div></div></body></html>");
                sb.Append(@"<html><head></head><body><div style='border:1px solid green;'><div style='padding: 20px 20px; background-color: #fff06769;'><p>Hi Balamanikandan Subramaniam,</p><p>Almost the end of the week! </p></div><div style='background-color: #f8f7f7;padding: 20px 20px;'><div style='padding: 20px 20px;border:1px solid white;background-color: white !important;'><p>Dear concern</p><p>Vendor Registration Link Initiliazed</p><div style='text-align: end;'>"+"<a href =\"" + siteURL + "/#/register/vendor?token=" + code + "&Id=" + TransID + "&Email=" + toEmail + "\"" + "><button style='width: 90px;height: 28px;backgroud-color:red;background-color: #008CBA;color: white'>Register</button></a>" + "</div></div></div></div></body></html>");
                subject = "Vendor Registration Initialization";
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = true;
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);
                MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                await client.SendMailAsync(reportEmail);
                return Ok("Mail sent Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest("Mail Not Sent");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateVendorOnBoarding(BPVendorOnBoardingView VendorOnBoarding)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _VendorOnBoardingRepository.CreateVendorOnBoarding(VendorOnBoarding);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/CreateVendorOnBoarding", ex);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateVendorOnBoarding(BPVendorOnBoardingView VendorOnBoarding)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _VendorOnBoardingRepository.UpdateVendorOnBoarding(VendorOnBoarding);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/UpdateVendorOnBoarding", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteVendorOnBoarding(BPVendorOnBoarding VendorOnBoarding)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _VendorOnBoardingRepository.DeleteVendorOnBoarding(VendorOnBoarding);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/DeleteVendorOnBoarding", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ApproveVendor(BPVendorOnBoarding VendorOnBoarding)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _VendorOnBoardingRepository.ApproveVendor(VendorOnBoarding);
                if (result != null)
                {
                    bool status = CreateXMLFromVendorOnBoarding(result);
                    bool status1 = SendAllAttachmentsToFTP(result);

                    if (status)
                    {
                        WriteLog.WriteToFile("Registration/ApproveVendor", "Approved Vendor details uploaded to FTP");
                    }
                    else
                    {
                        WriteLog.WriteToFile("Registration/ApproveVendor", "Approved Vendor details not uploaded to FTP");
                    }
                    if (status1)
                    {
                        WriteLog.WriteToFile("Registration/ApproveVendor", "Attachments uploaded to FTP");
                    }
                    else
                    {
                        WriteLog.WriteToFile("Registration/ApproveVendor", "Attachments not uploaded to FTP");
                    }
                    if(status && status1)
                    {
                        await _VendorOnBoardingRepository.UpdateVendorOnBoardingStatus(VendorOnBoarding, "Approved");
                    }
                    else
                    {
                        await _VendorOnBoardingRepository.UpdateVendorOnBoardingStatus(VendorOnBoarding, "Registered");
                        BadRequest("Something Went Wrong Please Try again");
                    }
                }
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/ApproveVendor", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult ApproveVendor_MSMEFTP(int TransID, int MSME_ID, int RP_ID, int TDS)
        {
            try
            {
                bool status2 = attachment_MSME(TransID, MSME_ID);
                bool status3 = attachment_RP_ID(TransID, RP_ID);
                bool status = attachment_TDS(TransID, TDS);
                if (status2)
                {
                    WriteLog.WriteToFile("Registration/ApproveVendor", "Approved Vendor details uploaded to FTP");
                }
                else
                {
                    WriteLog.WriteToFile("Registration/ApproveVendor", "Approved Vendor details not uploaded to FTP");
                }
                return Ok(status2);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult ApproveVendor_Bank(int TransID)
        {

            try
            {
                var bank = _VendorOnBoardingRepository.bank(TransID);
                var bank1 = _VendorOnBoardingRepository.bank_doc(TransID, bank[0].AccountNo, bank[0].AttachmentName);

                var doc = bank_FTP(TransID, bank1[0].AttachmentFile, bank1[0].AttachmentName);

                return Ok(bank1);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }







        [HttpPost]
        public async Task<IActionResult> RejectVendor(BPVendorOnBoarding VendorOnBoarding)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _VendorOnBoardingRepository.RejectVendor(VendorOnBoarding);
                //if (result != null)
                //{
                //    bool status = CreateXMLFromVendorOnBoarding(result);
                //    if (status)
                //    {
                //        WriteLog.WriteToFile("Registration/RejectVendor", "Rejected Vendor details uploaded to FTP");
                //    }
                //    else
                //    {
                //        WriteLog.WriteToFile("Registration/RejectVendor", "Rejected Vendor details not uploaded to FTP");
                //    }
                //}
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/RejectVendor", ex);
                return BadRequest(ex.Message);
            }
        }


        public async Task<IActionResult> DeleteVendorOnboardingById(string Transid)
        {
            try
            {
                if (Transid != null)
                {
                    await _VendorOnBoardingRepository.DeleteVendorOnboardingById(Transid);
                    return Ok("Deleted");
                }
                return BadRequest("Not deleted");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region MSME ATTACHMENT
        public bool attachment_MSME(int Transid, int MSME_ID)
        {
            try
            {
                bool status = false;
                WriteLog.WriteToFile("Registration/attachmentFtp", "------attachmentFtp method started------");
                CreateVendorTempFolder();
                Random r = new Random();
                int num = r.Next(1, 9999999);
                string writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VendorTempFolder");


                var Attachment = _VendorOnBoardingRepository.GetAttachmentforXML_MSME(MSME_ID);

                for (int i = 0; i < Attachment.Count; i++)
                {
                    if (Attachment[i].AttachmentFile.Length != 0)
                    {
                        var FileName = Transid + "_" + "MSME" + "_" + Attachment[i].AttachmentName;
                        var FileFullPath = Path.Combine(writerFolder, FileName);
                        System.IO.File.WriteAllBytes(FileFullPath, Attachment[i].AttachmentFile);
                        string writerPath = Path.Combine(writerFolder, FileName);


                        WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "Created  XML file with Vendor");

                        var uploadStatus = UploadFileToVendorOutputFolder(writerFolder, FileName);

                        if (uploadStatus == true)
                        {
                            status = true;
                            WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "UploadFileToVendorOutputFolder Success");
                        }
                        else
                        {
                            status = false;
                            WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "UploadFileToVendorOutputFolder Failure");
                        }

                        WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "------CreateXMLFromVendorOnBoarding method ended------");

                    }
                    else
                    {
                        WriteLog.WriteToFile("There is no File in attachment ");
                    }

                }



                return true;

            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/attachmentFtp/Exception", ex.Message);
                return false;
            }

        }
        [HttpGet]
        public BPAttachment GetBPAttachmentByAttachmentId(int attachAttachmentId)
        {
            try
            {
                var Attachment = _VendorOnBoardingRepository.GetBPAttachmentByAttachmentId(attachAttachmentId);
                return Attachment;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetBPAttachmentByAttachmentId", ex);
                return null;
            }
        }
        #endregion


        #region bank_FTP

        public bool bank_FTP(int TransID, Byte[] AttachmentFile, string AttachmentName)
        {
            try
            {
                bool status = false;
                WriteLog.WriteToFile("Registration/attachmentFtp", "------attachmentFtp method started------");
                CreateVendorTempFolder();
                Random r = new Random();
                int num = r.Next(1, 9999999);
                string writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VendorTempFolder");


                //var Attachment = _VendorOnBoardingRepository.GetAttachmentforXML_RP_ID(RP_ID);

                //for (int i = 0; i < Attachment.Count; i++)
                //{
                if (AttachmentFile.Length != 0)
                {
                    var FileName = TransID + "_" + "BANK" + "_" + AttachmentName;
                    var FileFullPath = Path.Combine(writerFolder, FileName);
                    System.IO.File.WriteAllBytes(FileFullPath, AttachmentFile);
                    string writerPath = Path.Combine(writerFolder, FileName);


                    WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "Created  XML file with Vendor");

                    var uploadStatus = UploadFileToVendorOutputFolder(writerFolder, FileName);

                    if (uploadStatus == true)
                    {
                        status = true;
                        WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "UploadFileToVendorOutputFolder Success");
                    }
                    else
                    {
                        status = false;
                        WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "UploadFileToVendorOutputFolder Failure");
                    }

                    WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "------CreateXMLFromVendorOnBoarding method ended------");

                }
                else
                {
                    WriteLog.WriteToFile("There is no File in attachment ");
                }

                //}



                return true;

            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/attachmentFtp/Exception", ex.Message);
                return false;
            }
        }

        #endregion
        #region  attachment_RP_ID  
        public bool attachment_RP_ID(int Transid, int RP_ID)
        {

            try
            {
                bool status = false;
                WriteLog.WriteToFile("Registration/attachmentFtp", "------attachmentFtp method started------");
                CreateVendorTempFolder();
                Random r = new Random();
                int num = r.Next(1, 9999999);
                string writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VendorTempFolder");


                var Attachment = _VendorOnBoardingRepository.GetAttachmentforXML_RP_ID(RP_ID);

                for (int i = 0; i < Attachment.Count; i++)
                {
                    if (Attachment[i].AttachmentFile.Length != 0)
                    {
                        var FileName = Transid + "_" + "RP_ID" + "_" + Attachment[i].AttachmentName;
                        var FileFullPath = Path.Combine(writerFolder, FileName);
                        System.IO.File.WriteAllBytes(FileFullPath, Attachment[i].AttachmentFile);
                        string writerPath = Path.Combine(writerFolder, FileName);


                        WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "Created  XML file with Vendor");

                        var uploadStatus = UploadFileToVendorOutputFolder(writerFolder, FileName);

                        if (uploadStatus == true)
                        {
                            status = true;
                            WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "UploadFileToVendorOutputFolder Success");
                        }
                        else
                        {
                            status = false;
                            WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "UploadFileToVendorOutputFolder Failure");
                        }

                        WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "------CreateXMLFromVendorOnBoarding method ended------");

                    }
                    else
                    {
                        WriteLog.WriteToFile("There is no File in attachment ");
                    }

                }



                return true;

            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/attachmentFtp/Exception", ex.Message);
                return false;
            }





        }



        #endregion
        #region attachment_TDS
        public bool attachment_TDS(int Transid, int TDS)
        {

            try
            {
                bool status = false;
                WriteLog.WriteToFile("Registration/attachmentFtp", "------attachmentFtp method started------");
                CreateVendorTempFolder();
                Random r = new Random();
                int num = r.Next(1, 9999999);
                string writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VendorTempFolder");


                var Attachment = _VendorOnBoardingRepository.GetAttachmentforXML_TDS(TDS);

                for (int i = 0; i < Attachment.Count; i++)
                {
                    if (Attachment[i].AttachmentFile.Length != 0)
                    {
                        var FileName = Transid + "_" + "TDS" + "_" + Attachment[i].AttachmentName;
                        var FileFullPath = Path.Combine(writerFolder, FileName);
                        System.IO.File.WriteAllBytes(FileFullPath, Attachment[i].AttachmentFile);
                        string writerPath = Path.Combine(writerFolder, FileName);


                        WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "Created  XML file with Vendor");

                        var uploadStatus = UploadFileToVendorOutputFolder(writerFolder, FileName);

                        if (uploadStatus == true)
                        {
                            status = true;
                            WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "UploadFileToVendorOutputFolder Success");
                        }
                        else
                        {
                            status = false;
                            WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "UploadFileToVendorOutputFolder Failure");
                        }

                        WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "------CreateXMLFromVendorOnBoarding method ended------");

                    }
                    else
                    {
                        WriteLog.WriteToFile("There is no File in attachment ");
                    }

                }



                return true;

            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/attachmentFtp/Exception", ex.Message);
                return false;
            }





        }


        #endregion

        #region 
        public bool SendAllAttachmentsToFTP(BPVendorOnBoarding bPVendorOnBoarding)
        {
            try
            {
                bool status = false;
                WriteLog.WriteToFile("Registration/SendAllAttachmentsToFTP", "------AttachmentFtp method started------");
                CreateVendorTempFolder();
                Random r = new Random();
                int num = r.Next(1, 9999999);
                string writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VendorTempFolder");
                var Attachment = _VendorOnBoardingRepository.GetAllAttachmentsToFTP(bPVendorOnBoarding);

                for (int i = 0; i < Attachment.Count; i++)
                {
                    if (Attachment[i].AttachmentFile != null && Attachment[i].AttachmentFile.Length > 0)
                    {
                        var FileName = bPVendorOnBoarding.TransID + "_" + Attachment[i].Type + "_" + Attachment[i].AttachmentName;
                        var FileFullPath = Path.Combine(writerFolder, FileName);
                        System.IO.File.WriteAllBytes(FileFullPath, Attachment[i].AttachmentFile);
                        WriteLog.WriteToFile("Registration/SendAllAttachmentsToFTP", $"------File {FileName} added in VendorTempFolder------");
                    }
                    else
                    {
                        WriteLog.WriteToFile($"File { Attachment[i].AttachmentName} doesn't have any content");
                    }

                }

                WriteLog.WriteToFile("Registration/SendAllAttachmentsToFTP", "FTP File upload about to start");

                var uploadStatus = UploadFileToVendorOutputFolder(writerFolder, "FTPFiles");

                if (uploadStatus == true)
                {
                    status = true;
                    WriteLog.WriteToFile("Registration/SendAllAttachmentsToFTP", "UploadFileToVendorOutputFolder Success");
                }
                else
                {
                    status = false;
                    WriteLog.WriteToFile("Registration/SendAllAttachmentsToFTP", "UploadFileToVendorOutputFolder Failure");
                }
                return status;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/attachmentFtp/Exception", ex.Message);
                return false;
            }
        }
        #endregion


        #region Approve/Reject Vendor Upload the details to FTP

        public bool CreateXMLFromVendorOnBoarding(BPVendorOnBoarding bPVendorOnBoarding)
        {
            try
            {
                bool status = false;
                if (bPVendorOnBoarding != null)
                {
                    WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "------CreateXMLFromVendorOnBoarding method started------");
                    CreateVendorTempFolder();
                    Random r = new Random();
                    int num = r.Next(1, 9999999);
                    string writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VendorTempFolder");
                    var FileName = bPVendorOnBoarding.TransID + ".xml";
                    //var FileName = bPVendorOnBoarding.TransID+"_" + num +"_" + ".xml";

                    string writerPath = Path.Combine(writerFolder, FileName);
                    XmlWriter writer = XmlWriter.Create(writerPath);
                    var Identitys = _IdentityRepository.GetIdentitiesByVOB(bPVendorOnBoarding.TransID);
                    var Banks = _BankRepository.GetBanksByVOB(bPVendorOnBoarding.TransID);
                    var Contacts = _ContactRepository.GetContactsByVOB(bPVendorOnBoarding.TransID);

                    WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "XML file fetching the Vendor details");
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Vendor");
                    writer.WriteElementString("NAME", bPVendorOnBoarding.Name);
                    if (bPVendorOnBoarding.LegalName.Split().Length > 2)
                    {
                        var names = bPVendorOnBoarding.LegalName.Split();
                        writer.WriteElementString("LEGAL_NAME", names[0]+" "+ names[1]);
                    }
                    else
                    {
                        writer.WriteElementString("LEGAL_NAME", bPVendorOnBoarding.LegalName);
                    }
                    writer.WriteElementString("ADDRESS_LINE1", bPVendorOnBoarding.AddressLine1);
                    writer.WriteElementString("ADDRESS_LINE2", bPVendorOnBoarding.AddressLine2);
                    writer.WriteElementString("CITY", bPVendorOnBoarding.City);
                    writer.WriteElementString("STATE", bPVendorOnBoarding.State);
                    writer.WriteElementString("COUNTRY", bPVendorOnBoarding.Country);
                    writer.WriteElementString("PINCODE", bPVendorOnBoarding.PinCode);
                    writer.WriteElementString("PANNumber", bPVendorOnBoarding.PANNumber);
                    writer.WriteElementString("PHONE1", bPVendorOnBoarding.Phone1 ?? "");
                    writer.WriteElementString("PHONE2", bPVendorOnBoarding.Phone2 ?? "");
                    writer.WriteElementString("EMAIL1", bPVendorOnBoarding.Email1 ?? "");
                    writer.WriteElementString("EMAIL2", bPVendorOnBoarding.Email2 ?? "");
                    writer.WriteElementString("ROLE", bPVendorOnBoarding.Role ?? "");
                    writer.WriteElementString("TYPE", bPVendorOnBoarding.Type ?? "");
                    writer.WriteElementString("ACCOUNT_GROUP", bPVendorOnBoarding.AccountGroup ?? "");
                    writer.WriteElementString("PURCHASE_ORG", bPVendorOnBoarding.PurchaseOrg ?? "");
                    writer.WriteElementString("COMPANY_CODE", bPVendorOnBoarding.CompanyCode ?? "");
                    writer.WriteElementString("DEPARTMENT", bPVendorOnBoarding.Department ?? "");
                    writer.WriteElementString("MSME_TYPE", bPVendorOnBoarding.MSME_TYPE ?? ""); 
                    writer.WriteElementString("GSTNumber", bPVendorOnBoarding.GSTNumber ?? "");
                    writer.WriteElementString("GSTStatus", bPVendorOnBoarding.GSTStatus ?? "");
                    writer.WriteElementString("TypeofIndustry", bPVendorOnBoarding.TypeofIndustry ?? "");
                    //writer.WriteElementString("VENDOR_CODE", bPVendorOnBoarding.VendorCode ?? "");
                    //writer.WriteElementString("CREATED_ON", bPVendorOnBoarding.CreatedOn.ToString("yyyyMMdd HH:mm:ss") ?? "");

                    if (Identitys != null && Identitys.Count > 0)
                    {
                        writer.WriteStartElement("Identity");
                        WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "Vendor Identities count" + Identitys.Count);
                        foreach (var identity in Identitys)
                        {
                            writer.WriteStartElement("Item");
                            writer.WriteElementString("TYPE", identity.Type ?? "");
                            //writer.WriteElementString("ID_NUMBER", identity.IDNumber ?? "");
                            writer.WriteElementString("VALID_UNTIL", identity.ValidUntil.HasValue ? identity.ValidUntil.Value.ToString("yyyyMMdd HH:mm:ss") : "");

                            writer.WriteElementString("ATTACHMENT_NAME", bPVendorOnBoarding.TransID + "_" + identity.Type + "_" + identity.AttachmentName);
                            //writer.WriteElementString("CREATED_ON", identity.CreatedOn.ToString("yyyyMMdd HH:mm:ss") ?? "");
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }

                    if (Banks != null && Banks.Count > 0)
                    {
                        writer.WriteStartElement("Bank");
                        WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "Vendor Banks count" + Banks.Count);
                        int i = 1;
                        foreach (var bank in Banks)
                        {
                            writer.WriteStartElement("Item");
                            writer.WriteElementString("B_NAME", bank.Name ?? "");
                            writer.WriteElementString("ACCOUNT_NO", bank.AccountNo ?? "");
                            writer.WriteElementString("BANK_NAME", bank.BankName ?? "");
                            writer.WriteElementString("BRANCH", bank.Branch ?? "");
                            writer.WriteElementString("IFSC", bank.IFSC ?? "");
                            writer.WriteElementString("B_CITY", bank.City ?? "");
                            //writer.WriteElementString("ATTACHMENT_NAME", bPVendorOnBoarding.TransID + "_" + "Bank" + i + "_" + bank.AttachmentName);
                            //writer.WriteElementString("CREATED_ON", bank.CreatedOn.ToString("yyyyMMdd HH:mm:ss") ?? "");
                            writer.WriteEndElement();
                            i++;
                        }
                        writer.WriteEndElement();
                    }

                    if (Contacts != null && Contacts.Count > 0)
                    {
                        writer.WriteStartElement("Contact");
                        WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "Vendor Contacts count" + Contacts.Count);
                        foreach (var contact in Contacts)
                        {
                            writer.WriteStartElement("Item");
                            writer.WriteElementString("C_NAME", contact.Name ?? "");
                            writer.WriteElementString("DEPARTMENT", contact.Department ?? "");
                            writer.WriteElementString("TITLE", contact.Title ?? "");
                            writer.WriteElementString("MOBILE", contact.Mobile ?? "");
                            writer.WriteElementString("EMAIL", contact.Email ?? "");
                            //writer.WriteElementString("CREATED_ON", contact.CreatedOn.ToString("yyyyMMdd HH:mm:ss") ?? "");
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Flush();
                    writer.Close();
                    WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "Created  XML file with Vendor");

                    var uploadStatus = UploadFileToVendorOutputFolder(writerFolder, FileName);
                    if (uploadStatus == true)
                    {
                        status = true;
                        WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "UploadFileToVendorOutputFolder Success");
                        //foreach (var grp in Invoices)
                        //{
                        //    foreach (var item1 in grp)
                        //    {
                        //        var update = (from tb in _ctx.P_INV_HEADER_DETAILS
                        //                      where tb.HEADER_ID.ToString() == item1.HEADER_ID
                        //                      select tb).FirstOrDefault();
                        //        update.ISXMLCREATED = true;
                        //        DateTime now = DateTime.Now;
                        //        update.XMLMOVED_ON = now;
                        //        await _ctx.SaveChangesAsync();
                        //        WriteLog.WriteToFile(string.Format("Successfully updated XMLCREATED Status for Invoice {0} in DB", update.INV_NO));
                        //    }
                        //}
                    }
                    else
                    {
                        status = false;
                        WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "UploadFileToVendorOutputFolder Failure");
                    }
                    //var identityItems = new List<IdentityItem>();
                    //foreach (var x in Identitys)
                    //{
                    //    var it = new IdentityItem
                    //    {
                    //        Type = x.Type,
                    //        IDNumber = x.IDNumber.ToString(),
                    //        DocID = x.DocID.ToString(),
                    //        ValidUntil = x.ValidUntil,
                    //        CreatedOn = x.CreatedOn
                    //        //POD_DATE = x.tb.POD_DATE.HasValue ? x.tb.POD_DATE.Value.ToString("yyyyMMdd") : "",
                    //        //POD_TIME = x.tb.POD_DATE.HasValue ? x.tb.POD_DATE.Value.ToString("HH:mm:ss") : "",
                    //        //VEHICLE_REPORTED_DATE = x.tb.VEHICLE_REPORTED_DATE.HasValue ? x.tb.VEHICLE_REPORTED_DATE.Value.ToString("yyyyMMdd HH:mm:ss") : "",
                    //        //TRANS_DAMAGE_REMARKS = string.IsNullOrEmpty(x.tb1.REASON) ? "" : x.tb1.REASON.ToLower() == "damaged" ? x.tb1.REMARKS : "",
                    //        //ACTUAL_DELIVERY_DATE = x.tb.ACTUAL_DELIVERY_DATE.HasValue ? x.tb.ACTUAL_DELIVERY_DATE.Value.ToString("yyyyMMdd HH:mm:ss") : "",
                    //        //DELIVERY_QTY = x.tb1.QUANTITY,
                    //        //POD_QTY = x.tb1.RECEIVED_QUANTITY,
                    //        //POD_DOC_UPLOAD_DATE = x.tb.POD_DATE.HasValue ? x.tb.POD_DATE.Value.ToString("yyyyMMdd") : "",
                    //        //POD_DOC_UPLOAD_TIME = x.tb.POD_DATE.HasValue ? x.tb.POD_DATE.Value.ToString("HH:mm:ss") : "",
                    //        //POD_EVENT_DATE = x.tb.PROPOSED_DELIVERY_DATE,
                    //        //POD_EVENT_TIME = x.tb.PROPOSED_DELIVERY_DATE,
                    //    };
                    //    //var attachDate = (from tb in _ctx.P_INV_ATTACHMENT
                    //    //                  where tb.HEADER_ID.ToString() == it.HEADER_ID
                    //    //                  orderby tb.CREATED_ON descending
                    //    //                  select tb.CREATED_ON).FirstOrDefault();
                    //    //if (attachDate != null)
                    //    //{
                    //    //    it.POD_DOC_UPLOAD_DATE = attachDate.ToString("dd.MM.yyyy");
                    //    //    it.POD_DOC_UPLOAD_TIME = attachDate.ToString("hh:mm:ss tt");
                    //    //}
                    //    identityItems.Add(it);
                    //}
                    //var identityItemss = identityItems.GroupBy(x => x.TransID).ToList();
                    WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "------CreateXMLFromVendorOnBoarding method ended------");
                }
                return status;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding/Exception", ex.Message);
                return false;
            }

        }

        public void CreateVendorTempFolder()
        {
            try
            {
                string path1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VendorTempFolder");
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(path1);
                }
                else
                {
                    if (Directory.GetFiles(path1).Length > 0) //if file found in folder
                    {
                        string[] txtList = Directory.GetFiles(path1, "*.xml");
                        foreach (string f in txtList)
                        {
                            System.GC.Collect();
                            System.GC.WaitForPendingFinalizers();
                            System.IO.File.Delete(f);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/CreateVendorTempFolder/Exception", ex.Message);
            }
        }

        public bool UploadFileToVendorOutputFolder(string filePath, string fileName)
        {
            bool status = false;
            try
            {
                IConfiguration FTPDetailsConfig = _configuration.GetSection("FTPDetails");
                string FTPOutbox = FTPDetailsConfig.GetValue<string>("Outbox");
                string FTPUsername = FTPDetailsConfig.GetValue<string>("Username");
                string FTPPassword = FTPDetailsConfig.GetValue<string>("Password");
                using (WebClient client = new WebClient())
                {
                    if (Directory.GetFiles(filePath).Length > 0) //if file found in folder
                    {
                        DirectoryInfo dir = new DirectoryInfo(filePath);
                        FileInfo[] files = dir.GetFiles();
                        foreach (var file in files)
                        {
                            if (file.Length > 0)
                            {
                                client.Credentials = new NetworkCredential(FTPUsername, FTPPassword);
                                byte[] responseArray = client.UploadFile(FTPOutbox + file.Name, file.FullName);
                                WriteLog.WriteToFile("Registration/UploadFileToVendorOutputFolder", "File uploaded to Vendor Output folder");
                                status = true;
                                WriteLog.WriteToFile("Registration/UploadFileToVendorOutputFolder", string.Format("File {0} was successfully uploaded to FTP {1}", file.Name, FTPOutbox));
                                System.IO.File.Delete(file.FullName);
                                //return status;
                            }
                            else
                            {
                                status = false;
                                WriteLog.WriteToFile("Registration/UploadFileToVendorOutputFolder", string.Format("File {0} has no contents", file.FullName));
                            }
                        }
                    }
                }
                return status;
            }

            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/UploadFileToVendorOutputFolder/Exception", ex.Message);
                return false;
            }
        }

        #endregion
        #region  postingAttachmentToFTP
        //public bool PostingAttachmentToFtp(BPVendorOnBoarding result)
        //  {

        //      WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding", "------CreateXMLFromVendorOnBoarding method started------");
        //      CreateVendorTempFolder();
        //      Random r = new Random();
        //      int num = r.Next(1, 9999999);
        //      string writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VendorTempFolder");
        //      var FileName = "Vendor" + num + ".xml";
        //      return 0;
        //  }
        #endregion

        #region BPIdentity

        [HttpGet]
        public List<BPIdentity> GetAllIdentities()
        {
            try
            {
                var Identitys = _IdentityRepository.GetAllIdentities();
                return Identitys;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetAllIdentities", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPIdentity> GetIdentitiesByVOB(int TransID)
        {
            try
            {
                var Identitys = _IdentityRepository.GetIdentitiesByVOB(TransID);
                return Identitys;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetIdentitiesByVOB", ex);
                return null;
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateIdentity(BPIdentity Identity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _IdentityRepository.CreateIdentity(Identity);
                return CreatedAtAction(nameof(GetAllIdentities), new { TransID = Identity.TransID, Type = Identity.Type }, Identity);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/CreateIdentity", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateIdentity(BPIdentity Identity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _IdentityRepository.UpdateIdentity(Identity);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/UpdateIdentity", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIdentity(BPIdentity Identity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _IdentityRepository.DeleteIdentity(Identity);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/DeleteIdentity", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region BPBank

        [HttpGet]
        public List<BPBank> GetAllBanks()
        {
            try
            {
                var Banks = _BankRepository.GetAllBanks();

                return Banks;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetAllBanks", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPBank> GetBanksByVOB(int TransID)
        {
            try
            {
                var Banks = _BankRepository.GetBanksByVOB(TransID);
                return Banks;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetBanksByVOB", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBank(BPBank Bank)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _BankRepository.CreateBank(Bank);
                return CreatedAtAction(nameof(GetAllBanks), new { TransID = Bank.TransID, AccountNo = Bank.AccountNo }, Bank);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/CreateBank", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBank(BPBank Bank)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _BankRepository.UpdateBank(Bank);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/UpdateBank", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBank(BPBank Bank)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _BankRepository.DeleteBank(Bank);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/DeleteBank", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region BPContact

        [HttpGet]
        public List<BPContact> GetAllContacts()
        {
            try
            {
                var Contacts = _ContactRepository.GetAllContacts();
                return Contacts;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetAllContacts", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPContact> GetContactsByVOB(int TransID)
        {
            try
            {
                var Contacts = _ContactRepository.GetContactsByVOB(TransID);
                return Contacts;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetContactsByVOB", ex);
                return null;
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateContact(BPContact Contact)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _ContactRepository.CreateContact(Contact);
                return CreatedAtAction(nameof(GetAllContacts), new { TransID = Contact.TransID, Item = Contact.Item }, Contact);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/CreateContact", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateContact(BPContact Contact)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _ContactRepository.UpdateContact(Contact);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/UpdateContact", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteContact(BPContact Contact)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _ContactRepository.DeleteContact(Contact);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/DeleteContact", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Reminder

        [HttpGet]
        public IActionResult SendReminderToInitializedVendor()
        {
            try
            {
                _ServiceRepository.SendReminderToInitializedVendor();
                return Ok();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/SendReminderToInitializedVendor", ex);
                return null;
            }
        }

        [HttpGet]
        public IActionResult SendReminderToSavedVendor()
        {
            try
            {
                _ServiceRepository.SendReminderToSavedVendor();
                return Ok();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/SendReminderToSavedVendor", ex);
                return null;
            }
        }

        [HttpGet]
        public IActionResult SendReminderToRegisteredVendor()
        {
            try
            {
                _ServiceRepository.SendReminderToRegisteredVendor();
                return Ok();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/SendReminderToRegisteredVendor", ex);
                return null;
            }
        }

        #endregion

        #region BPActivityLog

        [HttpGet]
        public List<BPActivityLog> GetAllActivityLogs()
        {
            try
            {
                var ActivityLogs = _ActivityLogRepository.GetAllActivityLogs();
                return ActivityLogs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetAllActivityLogs", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPActivityLog> GetActivityLogsByVOB(int TransID)
        {
            try
            {
                var ActivityLogs = _ActivityLogRepository.GetActivityLogsByVOB(TransID);
                return ActivityLogs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetActivityLogsByVOB", ex);
                return null;
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateActivityLog(BPActivityLog ActivityLog)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _ActivityLogRepository.CreateActivityLog(ActivityLog);
                return CreatedAtAction(nameof(GetAllActivityLogs), new { TransID = ActivityLog.TransID, LogID = ActivityLog.LogID }, ActivityLog);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/CreateActivityLog", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateActivityLog(BPActivityLog ActivityLog)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _ActivityLogRepository.UpdateActivityLog(ActivityLog);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/UpdateActivityLog", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteActivityLog(BPActivityLog ActivityLog)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _ActivityLogRepository.DeleteActivityLog(ActivityLog);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/DeleteActivityLog", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region BPText

        [HttpGet]
        public List<BPText> GetAllTexts()
        {
            try
            {
                var Texts = _TextRepository.GetAllTexts();
                return Texts;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetAllTexts", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateText(BPText Text)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _TextRepository.CreateText(Text);
                return CreatedAtAction(nameof(GetAllTexts), new { TextID = Text.TextID }, Text);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/CreateText", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateText(BPText Text)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _TextRepository.UpdateText(Text);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/UpdateText", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteText(BPText Text)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _TextRepository.DeleteText(Text);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/DeleteText", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Questions

        public QuestionnaireResultSet GetQuestionnaireResultSetByQRID()
        {
            try
            {
                var result = _VendorOnBoardingRepository.GetQuestionnaireResultSetByQRID();
                return result;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/GetQuestionnaireResultSetByQRID", ex);
                return null;
            }
        }
        #endregion

        #region FTP Methods

        //public void CreateXMLFromVendorOnBoarding(BPVendorOnBoarding bPVendorOnBoarding)
        //{
        //    try
        //    {

        //        string str = JsonConvert.SerializeObject(aNX1GetSummaryRes);
        //        XmlDocument xmlDoc = (System.Xml.XmlDocument)JsonConvert.DeserializeXmlNode(str, "root");
        //        if (xmlDoc != null)
        //        {
        //            xmlDoc.Save(temporaryReponseFolderPath + "\\" + xmlFilename);
        //            bool uploadStatus = UploadFileToFTP(temporaryReponseFolderPath, FTPOutbox, 0);
        //            if (uploadStatus)
        //            {
        //                DeleteFileFromTemporaryFolder(xmlFilename);
        //                DeleteFileFromReponseTemporaryFolder(xmlFilename);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //private bool UploadFileToFTP(string Source)
        //{
        //    bool uploadFileStatus = false;
        //    //Log.WriteLog("UploadFileToFTP:- Source path" + Destination);
        //    try
        //    {
        //        IConfiguration FTPDetailsConfig = _configuration.GetSection("FTPDetails");
        //        string Outbox = FTPDetailsConfig.GetValue<string>("Outbox");
        //        string Username = FTPDetailsConfig.GetValue<string>("Username");
        //        string Password = FTPDetailsConfig.GetValue<string>("Password");
        //        using (WebClient client = new WebClient())
        //        {
        //            if (Directory.GetFiles(Source).Length > 0) //if file found in folder
        //            {
        //                DirectoryInfo dir = new DirectoryInfo(Source);
        //                FileInfo[] files = dir.GetFiles();
        //                foreach (var file in files)
        //                {
        //                    if (file.Length > 0)
        //                    {
        //                        string name = "";
        //                        //if (version != 0)
        //                        //{
        //                        //    name = System.IO.Path.GetFileName(file.ToString());
        //                        //    name = name.Replace(".xml", "_v" + version + ".xml");
        //                        //}
        //                        //else
        //                        //{
        //                        name = System.IO.Path.GetFileName(file.ToString());
        //                        //}
        //                        client.Credentials = new NetworkCredential(Username, Password);
        //                        byte[] responseArray = client.UploadFile(Outbox + name, file.FullName);
        //                        uploadFileStatus = true;
        //                        //Log.WriteLog(string.Format("UploadFileToFTP:- Response Xml file {0} was successfully uploaded to FTP {1}", name, Destination));
        //                    }
        //                    else
        //                    {
        //                        uploadFileStatus = true;
        //                        //Log.WriteLog(string.Format("UploadFileToFTP:- File {0} has no contents", file.FullName));
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        uploadFileStatus = false;
        //        WriteLog.WriteToFile("UploadFileToFTP/Exception" , ex.Message);
        //    }
        //    return uploadFileStatus;
        //}

        //public static string[] GetFileListFromFTP(string FTPBox, string FTPusrname, string FTPPasswrd)
        //{
        //    StringBuilder result = new StringBuilder();
        //    WebResponse response = null;
        //    StreamReader reader = null;
        //    FtpWebRequest reqFTP = null;
        //    try
        //    {
        //        reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(FTPBox));
        //        reqFTP.UseBinary = true;
        //        reqFTP.Credentials = new NetworkCredential(FTPusrname, FTPPasswrd);
        //        reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
        //        reqFTP.Proxy = null;
        //        reqFTP.KeepAlive = false;
        //        reqFTP.UsePassive = true;
        //        reqFTP.EnableSsl = false;
        //        response = reqFTP.GetResponse();
        //        reader = new StreamReader(response.GetResponseStream());
        //        string line = reader.ReadLine();
        //        while (line != null)
        //        {
        //            result.Append(line);
        //            result.Append("\n");
        //            line = reader.ReadLine();
        //        }
        //        string resultedString = result.ToString();
        //        if (!string.IsNullOrEmpty(resultedString))
        //        {
        //            int lastIndex = resultedString.LastIndexOf('\n');
        //            if (lastIndex > -1)
        //            {
        //                resultedString = resultedString.Substring(0, resultedString.LastIndexOf('\n'));
        //            }
        //            var x = resultedString.Split('\n');
        //            return resultedString.Split('\n');
        //        }
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.WriteLog("GetFileListFromFTP/Exception:- " + ex);
        //        return null;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            reader.Close();
        //        }
        //        if (response != null)
        //        {
        //            response.Close();
        //        }
        //    }

        //}

        //private static string DownloadFromFTP(string FTPInbox, string FTPusrname, string FTPPasswrd, string file)
        //{
        //    try
        //    {
        //        FtpWebRequest reqFTP;
        //        reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(FTPInbox + "/" + file));
        //        reqFTP.Credentials = new NetworkCredential(FTPusrname, FTPPasswrd);
        //        reqFTP.KeepAlive = false;
        //        reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
        //        reqFTP.UseBinary = true;
        //        reqFTP.Proxy = null;
        //        reqFTP.UsePassive = true;
        //        FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
        //        Stream responseStream = response.GetResponseStream();
        //        string Invoice_Store = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Request_Folder") + "\\";
        //        FileStream writeStream = new FileStream(Invoice_Store + file, FileMode.Create);
        //        int Length = 2048;
        //        Byte[] buffer = new Byte[Length];
        //        int bytesRead = responseStream.Read(buffer, 0, Length);
        //        while (bytesRead > 0)
        //        {
        //            writeStream.Write(buffer, 0, bytesRead);
        //            bytesRead = responseStream.Read(buffer, 0, Length);
        //        }
        //        writeStream.Close();
        //        response.Close();
        //        return writeStream.Name;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.WriteLog("DownloadFromFTP/Exception:- " + ex.Message);
        //        return null;
        //    }
        //}

        //private static bool UploadErrorFileToFTP(string Source, string Destination, int version)
        //{
        //    bool uploadFileStatus = false;
        //    Log.WriteLog("UploadErrorFileToFTP:- Source path" + Destination);
        //    try
        //    {
        //        using (WebClient client = new WebClient())
        //        {
        //            if (Directory.GetFiles(Source).Length > 0) //if file found in folder
        //            {
        //                DirectoryInfo dir = new DirectoryInfo(Source);
        //                FileInfo[] files = dir.GetFiles();
        //                foreach (var file in files)
        //                {
        //                    if (file.Length > 0)
        //                    {
        //                        string name = "";
        //                        if (version != 0)
        //                        {
        //                            name = System.IO.Path.GetFileName(file.ToString());
        //                            name = name.Replace(".xml", "_v" + version + ".xml");
        //                        }
        //                        else
        //                        {
        //                            name = System.IO.Path.GetFileName(file.ToString());
        //                        }
        //                        client.Credentials = new NetworkCredential(FTPusrname, FTPPasswrd);
        //                        byte[] responseArray = client.UploadFile(Destination + name, file.FullName);
        //                        uploadFileStatus = true;
        //                        Log.WriteLog(string.Format("UploadErrorFileToFTP:- Request Xml file {0} was successfully uploaded to FTP {1}", name, Destination));
        //                    }
        //                    else
        //                    {
        //                        uploadFileStatus = true;
        //                        Log.WriteLog(string.Format("UploadErrorFileToFTP:- File {0} has no contents", file.FullName));
        //                    }

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        uploadFileStatus = false;
        //        Log.WriteLog("UploadErrorFileToFTP/Exception:- " + ex.Message);
        //    }
        //    return uploadFileStatus;
        //}

        //private static void DeleteFromFTP(string FTPVendorDetails, string FTPusrname, string FTPPasswrd, string file)
        //{
        //    try
        //    {

        //        FtpWebRequest reqFTP;
        //        reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(FTPVendorDetails + "/" + file));
        //        reqFTP.Credentials = new NetworkCredential(FTPusrname, FTPPasswrd);
        //        reqFTP.KeepAlive = false;
        //        reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
        //        reqFTP.UseBinary = true;
        //        reqFTP.Proxy = null;
        //        reqFTP.UsePassive = true;
        //        FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
        //        response.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.WriteLog("DeleteFromFTP/Exception:- " + ex.Message);
        //    }
        //}

        #endregion
    }
}