using BPCloud.VendorRegistrationService.DBContexts;
using BPCloud.VendorRegistrationService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BPCloud.VendorRegistrationService.Repositories
{
    public class VendorOnBoardingRepository : IVendorOnBoardingRepository
    {
        private readonly RegistrationContext _dbContext;
        IConfiguration _configuration;
        private object user;
        private int _tokenTimespan = 30;

        public VendorOnBoardingRepository(RegistrationContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            try
            {
                if (_configuration["TokenTimeSpan"] != "")
                    _tokenTimespan = Convert.ToInt32(_configuration["TokenTimeSpan"].ToString());
                if (_tokenTimespan <= 0)
                {
                    _tokenTimespan = 30;
                }
            }
            catch
            {
                _tokenTimespan = 30;
            }
        }

        public List<BPVendorOnBoarding> GetAllVendorOnBoardings()
        {
            try
            {
                return _dbContext.BPVendorOnBoardings.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPVendorOnBoarding> GetAllOpenVendorOnBoardings()
        {
            try
            {
                return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "registered").ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPVendorOnBoarding> GetAllApprovedVendorOnBoardings()
        {
            try
            {
                return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "approved").ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPVendorOnBoarding> GetAllRejectedVendorOnBoardings()
        {
            try
            {
                return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "rejected").ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public string GetVirtualPath(string physicalPath)
        //{
        //    try
        //    {
        //        if (!physicalPath.StartsWith(HttpContext.))
        //        {
        //            return null;
        //        }
        //        return "~/" + physicalPath.Substring(HttpContext.Current.Request.PhysicalApplicationPath.Length)
        //              .Replace("\\", "/");
        //        return physicalPath.Substring(HttpContext.Current.Request.PhysicalApplicationPath.Length)
        //             .Replace("\\", "/");
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.WriteToFile("AuthorizationServerProvider/GetVirtualPath/Exception:- " + ex.Message);
        //        return null;
        //    }
        //}
        public List<BPVendorOnBoarding> GetAllOpenVendorOnBoardingsByPlant(List<string> Plants)
        {
            try
            {
                TimeSpan ts = new TimeSpan(30, 0, 0, 0);
                var subractedTime = DateTime.Now.Subtract(ts);
                var result = _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "registered" && Plants.Any(y => y == x.Plant) && x.ModifiedOn >= subractedTime).ToList();
                //var result = _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "registered" && Plants.Any(y => y == x.Plant)).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPVendorOnBoarding> GetAllOpenVendorOnBoardingsByApprover(string Approver)
        {
            try
            {
                var result = _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "registered" && x.EmamiContactPerson == Approver).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPVendorOnBoarding> GetAllApprovedVendorOnBoardingsByPlant(List<string> Plants)
        {
            try
            {

                return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "approved" && Plants.Any(y => y == x.Plant)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPVendorOnBoarding> GetAllApprovedVendorOnBoardingsByApprover(string Approver)
        {
            try
            {

                return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "approved" && x.EmamiContactPerson == Approver).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPVendorOnBoarding> GetAllRejectedVendorOnBoardingsByPlant(List<string> Plants)
        {
            try
            {
                return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "rejected" && Plants.Any(y => y == x.Plant)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPVendorOnBoarding> GetAllRejectedVendorOnBoardingsByApprover(string Approver)
        {
            try
            {
                return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "rejected" && x.EmamiContactPerson == Approver).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPVendorOnBoarding> GetAllOpenVendorOnBoardingsCount()
        {
            try
            {
                return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "registered").ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPVendorOnBoarding> GetAllOpenVendorOnBoardingsCountByApprover(string Approver)
        {
            try
            {
                return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "registered" && x.EmamiContactPerson == Approver).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPVendorOnBoarding> GetAllApprovedVendorOnBoardingsCount()
        {
            try
            {
                return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "approved").ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPVendorOnBoarding> GetAllRejectedVendorOnBoardingsCount()
        {
            try
            {
                return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "rejected").ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPVendorOnBoarding> GetAllOpenVendorOnBoardingsCountByPlant(List<string> Plants)
        {
            try
            {
                return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "registered" && Plants.Any(y => y == x.Plant)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPVendorOnBoarding> GetAllApprovedVendorOnBoardingsCountByPlant(List<string> Plants)
        {
            try
            {
                return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "approved" && Plants.Any(y => y == x.Plant)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPVendorOnBoarding> GetAllRejectedVendorOnBoardingsCountByPlant(List<string> Plants)
        {
            try
            {
                return _dbContext.BPVendorOnBoardings.Where(x => x.Status.ToLower() == "rejected" && Plants.Any(y => y == x.Plant)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BPVendorOnBoarding GetVendorOnBoardingsByID(int TransID)
        {
            try
            {
                return _dbContext.BPVendorOnBoardings.Where(x => x.TransID == TransID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPIdentity> GetAllIdentity(int TransID)
        {
            try
            {
                return _dbContext.BPIdentities.Where(x => x.TransID == TransID).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public BPAttachment GetAttachmentFile(int TransID, string attachmentname)
        //public async Task<BPVendorOnBoarding> InitializeVendorRegistration(VendorInitialzationClass vendorInitialzationClass)
        public List<BPAttachment> GetAttachmentFile(int TransID, string attachmentname)
        {
            //TransID = 3027;
            //attachmentname = "BP.jpg";

            var result = (from tb in _dbContext.BPAttachments
                              //join tb1 in _dbContext.BPIdentity on tb.AttachmentName equals tb1.AttachmentName
                          where TransID.ToString() == tb.HeaderNumber
                              &&
                           attachmentname == tb.AttachmentName
                          select new BPAttachment()
                          {
                              AttachmentFile = tb.AttachmentFile,
                              AttachmentName = tb.AttachmentName
                          }).ToList();
            return result;
        }
        public List<BPAttachment1> GetAttachmentforXML(int transid)
        {
            try
            {
                var Identitys = (from tb in _dbContext.BPAttachments
                                 join tb1 in _dbContext.BPIdentities
                                 on tb.AttachmentName equals tb1.AttachmentName
                                 where tb.AppNumber == tb1.Type
                                 && tb1.TransID.ToString() == tb.HeaderNumber
                                 && tb1.TransID == transid
                                 select new BPAttachment1()
                                 {
                                     AttachmentFile = tb.AttachmentFile,
                                     AttachmentName = tb.AttachmentName,
                                     Type = tb1.Type
                                 }).ToList();
                return Identitys;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<FTPAttachment> GetAllAttachmentsToFTP(BPVendorOnBoarding bPVendorOnBoarding)
        {
            List<FTPAttachment> allAttachments = new List<FTPAttachment>();
            try
            {
                List<FTPAttachment> IndentityFTPAttachments = GetIdentityFTPAttachment(bPVendorOnBoarding.TransID);
                allAttachments.AddRange(IndentityFTPAttachments);
                List<FTPAttachment> BankFTPAttachments = GetBankFTPAttachment(bPVendorOnBoarding.TransID);
                allAttachments.AddRange(BankFTPAttachments);
                if (!string.IsNullOrEmpty(bPVendorOnBoarding.MSME_Att_ID))
                {
                    List<FTPAttachment> MSMEFTPAttachments = GetMSMEFTPAttachment(bPVendorOnBoarding.TransID, bPVendorOnBoarding.MSME_Att_ID);
                    allAttachments.AddRange(MSMEFTPAttachments);
                }
                if (!string.IsNullOrEmpty(bPVendorOnBoarding.RP_Att_ID))
                {
                    List<FTPAttachment> RPFTPAttachments = GetRPFTPAttachment(bPVendorOnBoarding.TransID, bPVendorOnBoarding.RP_Att_ID);
                    allAttachments.AddRange(RPFTPAttachments);
                }
                if (!string.IsNullOrEmpty(bPVendorOnBoarding.TDS_Att_ID))
                {
                    List<FTPAttachment> TDSFTPAttachments = GetTDSFTPAttachment(bPVendorOnBoarding.TransID, bPVendorOnBoarding.TDS_Att_ID);
                    allAttachments.AddRange(TDSFTPAttachments);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return allAttachments;
        }

        public List<FTPAttachment> GetIdentityFTPAttachment(int TransID)
        {
            try
            {
                var Identitys = (from tb in _dbContext.BPAttachments
                                 join tb1 in _dbContext.BPIdentities
                                 on tb.AttachmentName equals tb1.AttachmentName
                                 where tb.AppNumber == tb1.Type
                                 && tb1.TransID.ToString() == tb.HeaderNumber
                                 && tb1.TransID == TransID
                                 select new FTPAttachment()
                                 {
                                     AttachmentID = tb.AttachmentID,
                                     TransID = TransID.ToString(),
                                     AttachmentFile = tb.AttachmentFile,
                                     AttachmentName = tb.AttachmentName,
                                     Type = tb1.Type
                                 }).ToList();
                return Identitys;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<FTPAttachment> GetBankFTPAttachment(int TransID)
        {
            try
            {
                var Banks = (from tb in _dbContext.BPAttachments
                             join tb1 in _dbContext.BPBanks
                             on tb.AttachmentName equals tb1.AttachmentName
                             where tb.AppNumber == tb1.AccountNo
                             && tb1.TransID.ToString() == tb.HeaderNumber
                             && tb1.TransID == TransID
                             select new FTPAttachment()
                             {
                                 AttachmentID = tb.AttachmentID,
                                 TransID = TransID.ToString(),
                                 AttachmentFile = tb.AttachmentFile,
                                 AttachmentName = tb.AttachmentName,
                                 Type = "Bank"
                             }).ToList();
                int i = 1;
                foreach (var bank in Banks)
                {
                    bank.Type = bank.Type + i;
                    i++;
                }
                return Banks;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<FTPAttachment> GetMSMEFTPAttachment(int TransID, string MSME_Att_ID)
        {
            try
            {
                var MSME = (from tb in _dbContext.BPAttachments
                            where tb.AttachmentID.ToString() == MSME_Att_ID
                            select new FTPAttachment()
                            {
                                AttachmentID = tb.AttachmentID,
                                TransID = TransID.ToString(),
                                AttachmentFile = tb.AttachmentFile,
                                AttachmentName = tb.AttachmentName,
                                Type = "MSME"
                            }).ToList();

                return MSME;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<FTPAttachment> GetRPFTPAttachment(int TransID, string RP_Att_ID)
        {
            try
            {
                var MSME = (from tb in _dbContext.BPAttachments
                            where tb.AttachmentID.ToString() == RP_Att_ID
                            select new FTPAttachment()
                            {
                                AttachmentID = tb.AttachmentID,
                                TransID = TransID.ToString(),
                                AttachmentFile = tb.AttachmentFile,
                                AttachmentName = tb.AttachmentName,
                                Type = "RP"
                            }).ToList();

                return MSME;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<FTPAttachment> GetTDSFTPAttachment(int TransID, string TDS_Att_ID)
        {
            try
            {
                var MSME = (from tb in _dbContext.BPAttachments
                            where tb.AttachmentID.ToString() == TDS_Att_ID
                            select new FTPAttachment()
                            {
                                AttachmentID = tb.AttachmentID,
                                TransID = TransID.ToString(),
                                AttachmentFile = tb.AttachmentFile,
                                AttachmentName = tb.AttachmentName,
                                Type = "TDS"
                            }).ToList();

                return MSME;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPAttachment1> GetAttachmentforXML_MSME(int MSME_ID)
        {
            try
            {
                var MSME = (from tb in _dbContext.BPAttachments
                            where tb.AttachmentID == MSME_ID
                            select new BPAttachment1()
                            {
                                AttachmentFile = tb.AttachmentFile,
                                AttachmentName = tb.AttachmentName
                            }).ToList();

                return MSME;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPAttachment1> GetAttachmentforXML_RP_ID(int RP_ID)
        {
            try
            {
                var MSME = (from tb in _dbContext.BPAttachments
                            where tb.AttachmentID == RP_ID
                            select new BPAttachment1()
                            {
                                AttachmentFile = tb.AttachmentFile,
                                AttachmentName = tb.AttachmentName
                            }).ToList();

                return MSME;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPAttachment1> GetAttachmentforXML_TDS(int TDS)
        {
            try
            {
                var MSME = (from tb in _dbContext.BPAttachments
                            where tb.AttachmentID == TDS
                            select new BPAttachment1()
                            {
                                AttachmentFile = tb.AttachmentFile,
                                AttachmentName = tb.AttachmentName
                            }).ToList();

                return MSME;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPBank> bank(int TransID)
        {
            try
            {
                var Bank = (from tb in _dbContext.BPBanks
                            where tb.TransID == TransID
                            select new BPBank()
                            {
                                AccountNo = tb.AccountNo,
                                AttachmentName = tb.AttachmentName
                            }).ToList();

                return Bank;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPAttachment1> bank_doc(int TransID, string Accono, string Attachmentname)
        {
            try
            {
                var Bank = (from tb in _dbContext.BPAttachments
                            where tb.HeaderNumber == TransID.ToString() && tb.AttachmentName == Attachmentname && tb.AppNumber == Accono.ToString()
                            select new BPAttachment1()
                            {
                                AttachmentFile = tb.AttachmentFile,
                                AttachmentName = tb.AttachmentName

                            }).ToList();

                return Bank;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public BPVendorOnBoarding GetVendorOnBoardingsByEmailID(string EmailID)
        {
            try
            {
                return _dbContext.BPVendorOnBoardings.Where(x => x.Email1.ToLower() == EmailID.ToLower()).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPVendorOnBoarding> GetRegisteredVendorOnBoardings()
        {
            try
            {
                return _dbContext.BPVendorOnBoardings.Where(x => string.IsNullOrEmpty(x.Status)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPVendorOnBoarding> InitializeVendorRegistration(VendorInitialzationClass vendorInitialzationClass)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    //BPVendorOnBoarding history1 = (from tb in _dbContext.BPVendorOnBoardings
                    //                               where tb.Email1 == vendorInitialzationClass.Email
                    //                               select tb).FirstOrDefault();
                    //if (history1 == null)
                    //{
                    BPVendorOnBoarding VendorOnBoarding = new BPVendorOnBoarding();
                    VendorOnBoarding.Name = vendorInitialzationClass.Name;
                    VendorOnBoarding.Type = vendorInitialzationClass.Type;
                    VendorOnBoarding.Email1 = vendorInitialzationClass.Email;
                    VendorOnBoarding.GSTNumber = vendorInitialzationClass.GSTNumber;
                    VendorOnBoarding.Type = vendorInitialzationClass.Type;
                    VendorOnBoarding.Department = vendorInitialzationClass.Department;
                    VendorOnBoarding.Plant = vendorInitialzationClass.Plant;
                    VendorOnBoarding.Department = vendorInitialzationClass.Department;
                    VendorOnBoarding.AccountGroup = vendorInitialzationClass.AccountGroup;
                    VendorOnBoarding.PurchaseOrg = vendorInitialzationClass.PurchaseOrg;
                    VendorOnBoarding.CompanyCode = vendorInitialzationClass.CompanyCode;
                    //VendorOnBoarding.VendorCode = vendorInitialzationClass.VendorCode;
                    VendorOnBoarding.EmamiContactPerson = vendorInitialzationClass.EmamiContactPerson;
                    VendorOnBoarding.EmamiContactPersonMail = vendorInitialzationClass.EmamiContactPersonMail;
                    //BPVendorOnBoarding VendorOnBoarding = new BPVendorOnBoarding();
                    //VendorOnBoarding.Name = vendorInitialzationClass.Name;
                    //VendorOnBoarding.Type = vendorInitialzationClass.Type;
                    //VendorOnBoarding.Email1 = vendorInitialzationClass.Email;
                    //VendorOnBoarding.GSTNumber = vendorInitialzationClass.GSTNumber;
                    //VendorOnBoarding.Type = vendorInitialzationClass.Type;
                    //VendorOnBoarding.Department = vendorInitialzationClass.Department;
                    //VendorOnBoarding.Plant = vendorInitialzationClass.Plant;
                    //VendorOnBoarding.Department = vendorInitialzationClass.Department;
                    //VendorOnBoarding.AccountGroup = vendorInitialzationClass.AccountGroup;
                    //VendorOnBoarding.PurchaseOrg = vendorInitialzationClass.PurchaseOrg;
                    //VendorOnBoarding.CompanyCode = vendorInitialzationClass.CompanyCode;
                    //VendorOnBoarding.EmamiContactPerson = vendorInitialzationClass.EmamiContactPerson;
                    //VendorOnBoarding.EmamiContactPersonMail = vendorInitialzationClass.EmamiContactPersonMail;

                    //VendorOnBoarding.Status = "Initialized";
                    //VendorOnBoarding.IsActive = true;
                    //VendorOnBoarding.CreatedOn = DateTime.Now;
                    //var result = _dbContext.BPVendorOnBoardings.Add(VendorOnBoarding);
                    //await _dbContext.SaveChangesAsync();
                    //bool result1 = await CreateVendorInitilizationToken(result.Entity);
                    //if (result1)
                    //{
                    //    transaction.Commit();
                    //    transaction.Dispose();
                    //    return VendorOnBoarding;
                    //}
                    //transaction.Rollback();
                    //transaction.Dispose();
                    //throw new Exception("Unable to generate token");
                    VendorOnBoarding.Status = "Initialized";
                    VendorOnBoarding.IsActive = true;
                    VendorOnBoarding.CreatedOn = DateTime.Now;
                    var result = _dbContext.BPVendorOnBoardings.Add(VendorOnBoarding);
                    await _dbContext.SaveChangesAsync();
                    bool result1 = await CreateVendorInitilizationToken(result.Entity);
                    ApproverUser approverUser = new ApproverUser();
                    approverUser.UserName = vendorInitialzationClass.EmamiContactPerson;
                    approverUser.Email = vendorInitialzationClass.EmamiContactPersonMail;
                    bool result2 = await CreateApproverUser(approverUser);
                    if (result1)
                    {
                        transaction.Commit();
                        transaction.Dispose();
                        return VendorOnBoarding;
                    }
                    transaction.Rollback();
                    transaction.Dispose();
                    throw new Exception("Unable to generate token");
                    //}
                    //else
                    //{
                    //    throw new Exception("Vendor with same email address has existed");
                    //}
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private async Task<bool> CreateVendorInitilizationToken(BPVendorOnBoarding VendorOnBoarding)
        {
            try
            {
                DateTime ExpireDateTime = DateTime.Now.AddDays(_tokenTimespan);
                string code = Encrypt(VendorOnBoarding.TransID.ToString() + '|' + VendorOnBoarding.Plant + '|' + ExpireDateTime, true);
                string PortalAddress = _configuration["PortalAddress"];
                bool sendresult = await SendMail(HttpUtility.UrlEncode(code), VendorOnBoarding.Name, VendorOnBoarding.Email1, VendorOnBoarding.TransID.ToString(), PortalAddress);
                if (sendresult)
                {
                    try
                    {
                        TokenHistory history1 = (from tb in _dbContext.TokenHistories
                                                 where tb.TransID == VendorOnBoarding.TransID && !tb.IsUsed
                                                 select tb).FirstOrDefault();
                        if (history1 == null)
                        {
                            TokenHistory history = new TokenHistory()
                            {
                                TransID = VendorOnBoarding.TransID,
                                Token = code,
                                EmailAddress = VendorOnBoarding.Email1,
                                CreatedOn = DateTime.Now,
                                ExpireOn = ExpireDateTime,
                                IsUsed = false,
                                Comment = "Registration link has been sent successfully"
                            };
                            var result = _dbContext.TokenHistories.Add(history);
                        }
                        else
                        {
                            WriteLog.WriteToFile("VendorOnBoardingRepository/CreateVendorInitilizationToken:- Token already present, updating new token to the user whose mail id is " + VendorOnBoarding.Email1);
                            history1.Token = code;
                            history1.CreatedOn = DateTime.Now;
                            history1.ExpireOn = ExpireDateTime;
                        }
                        await _dbContext.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        WriteLog.WriteToFile("Master/SendLinkToMail/Exception:- Add record to TokenHistories - " + ex.Message, ex);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CreateApproverUser(ApproverUser approverUser)
        {
            try
            {
                string BaseAddress = _configuration.GetValue<string>("APIBaseAddress");
                string HostURI = BaseAddress + "/authenticationapi/Master/CreateApproverUser";
                var uri = new Uri(HostURI);
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                var SerializedObject = JsonConvert.SerializeObject(approverUser);
                byte[] requestBody = Encoding.UTF8.GetBytes(SerializedObject);

                using (var postStream = await request.GetRequestStreamAsync())
                {
                    await postStream.WriteAsync(requestBody, 0, requestBody.Length);
                }

                try
                {
                    using (var response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        if (response != null && response.StatusCode == HttpStatusCode.OK)
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            return true;
                        }
                        else
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            return false;
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var errorMessage = reader.ReadToEnd();
                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            //throw new Exception(errorMessage);
                            WriteLog.WriteToFile($"FactRepository/CreateVendorUser:- Error {errorMessage} for {approverUser.UserName}");
                            return false;
                        }
                        //throw ex;
                        WriteLog.WriteToFile($"FactRepository/CreateVendorUser:- Error {ex.Message} for {approverUser.UserName}");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    //throw ex;
                    WriteLog.WriteToFile($"FactRepository/CreateVendorUser:- Error {ex.Message} for {approverUser.UserName}");
                    return false;
                }

            }
            catch (Exception ex)
            {
                //throw ex; 
                WriteLog.WriteToFile($"FactRepository/CreateVendorUser:- Error {ex.Message} for {approverUser.UserName}");
                return false;
            }
        }


        public bool ChectTokenValidity(VendorTokenCheck tokenCheck)
        {
            string[] decryptedArray = new string[3];
            string result = string.Empty;
            try
            {
                try
                {
                    result = Decrypt(tokenCheck.Token, true);
                }
                catch
                {
                    throw new Exception("Invalid token!");
                }
                if (!string.IsNullOrEmpty(result) && result.Contains('|') && result.Split('|').Length == 3)
                {
                    decryptedArray = result.Split('|');
                }
                else
                {
                    throw new Exception("Invalid token!");
                }

                if (decryptedArray.Length == 3)
                {
                    DateTime date = DateTime.Parse(decryptedArray[2].Replace('+', ' '));
                    if (DateTime.Now > date)// Convert.ToDateTime(decryptedarray[2]))
                    {
                        throw new Exception("token expired!");
                    }
                    var DecryptedUserID = decryptedArray[0];

                    var user = (from tb in _dbContext.BPVendorOnBoardings
                                where tb.TransID.ToString() == DecryptedUserID && tb.IsActive
                                select tb).FirstOrDefault();

                    if (user.Plant == decryptedArray[1] && tokenCheck.TransID == user.TransID)
                    {
                        try
                        {
                            TokenHistory history = _dbContext.TokenHistories.Where(x => x.TransID == user.TransID && !x.IsUsed && x.Token == tokenCheck.Token).Select(r => r).FirstOrDefault();
                            if (history != null)
                            {
                                return true;
                            }
                            else
                            {
                                throw new Exception("Token might have already used or wrong token");

                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLog.WriteToFile("Master/ChectTokenValidity/Exception:- Getting TokenHistory - " + ex.Message, ex);
                            throw new Exception("Token might have already used or wrong token");
                        }

                    }
                    else
                    {
                        throw new Exception("Invalid token!");
                    }
                }
                else
                {
                    throw new Exception("Invalid token!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPVendorOnBoarding> CreateVendorOnBoarding(BPVendorOnBoardingView VendorOnBoardingView)
        {

            string[] decryptedArray = new string[3];
            string result2 = string.Empty;
            try
            {
                try
                {
                    result2 = Decrypt(VendorOnBoardingView.Token, true);
                }
                catch
                {
                    throw new Exception("Invalid token!");
                }
                if (result2.Contains('|') && result2.Split('|').Length == 3)
                {
                    decryptedArray = result2.Split('|');
                }
                else
                {
                    throw new Exception("Invalid token!");
                }

                if (decryptedArray.Length == 3)
                {
                    DateTime date = DateTime.Parse(decryptedArray[2].Replace('+', ' '));
                    if (DateTime.Now > date)// Convert.ToDateTime(decryptedarray[2]))
                    {
                        throw new Exception("token expired!");
                    }
                    var DecryptedUserID = decryptedArray[0];

                    var user = (from tb in _dbContext.BPVendorOnBoardings
                                where tb.TransID.ToString() == DecryptedUserID && tb.IsActive
                                select tb).FirstOrDefault();

                    if (user.Plant == decryptedArray[1] && VendorOnBoardingView.TransID == user.TransID)
                    {
                        try
                        {
                            TokenHistory history = _dbContext.TokenHistories.Where(x => x.TransID == user.TransID && !x.IsUsed && x.Token == VendorOnBoardingView.Token).Select(r => r).FirstOrDefault();
                            if (history != null)
                            {
                                //BPVendorOnBoarding vendorOnBoarding = new BPVendorOnBoarding();
                                //var strategy = _dbContext.Database.CreateExecutionStrategy();
                                //await strategy.ExecuteAsync(async () =>
                                //{
                                using (var transaction = _dbContext.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        var bPVendorOnBoarding = _dbContext.BPVendorOnBoardings.Where(x => x.Email1 == VendorOnBoardingView.Email1 && x.IsActive).FirstOrDefault();
                                        if (bPVendorOnBoarding == null)
                                        {
                                            BPVendorOnBoarding VendorOnBoarding = new BPVendorOnBoarding();
                                            VendorOnBoarding.Name = VendorOnBoardingView.Name;
                                            VendorOnBoarding.Role = VendorOnBoardingView.Role;
                                            VendorOnBoarding.LegalName = VendorOnBoardingView.LegalName;
                                            VendorOnBoarding.AddressLine1 = VendorOnBoardingView.AddressLine1;
                                            VendorOnBoarding.AddressLine2 = VendorOnBoardingView.AddressLine2;
                                            VendorOnBoarding.City = VendorOnBoardingView.City;
                                            VendorOnBoarding.State = VendorOnBoardingView.State;
                                            VendorOnBoarding.Country = VendorOnBoardingView.Country;
                                            VendorOnBoarding.PinCode = VendorOnBoardingView.PinCode;
                                            VendorOnBoarding.Type = VendorOnBoardingView.Type;
                                            VendorOnBoarding.Plant = VendorOnBoardingView.Plant;
                                            VendorOnBoarding.GSTNumber = VendorOnBoardingView.GSTNumber;
                                            VendorOnBoarding.GSTStatus = VendorOnBoardingView.GSTStatus;
                                            VendorOnBoarding.PANNumber = VendorOnBoardingView.PANNumber;
                                            VendorOnBoarding.Phone1 = VendorOnBoardingView.Phone1;
                                            VendorOnBoarding.Phone2 = VendorOnBoardingView.Phone2;
                                            VendorOnBoarding.Email1 = VendorOnBoardingView.Email1;
                                            VendorOnBoarding.Email2 = VendorOnBoardingView.Email2;
                                            //VendorOnBoarding.Invoice = VendorOnBoardingView.Invoice;
                                            //VendorOnBoarding.WebsiteAddress = VendorOnBoardingView.WebsiteAddress;
                                            //VendorOnBoarding.VendorCode = VendorOnBoardingView.VendorCode;
                                            //VendorOnBoarding.ParentVendor = VendorOnBoardingView.ParentVendor;
                                            VendorOnBoarding.Status = VendorOnBoardingView.Status;
                                            //VendorOnBoarding.ContactPerson = VendorOnBoardingView.ContactPerson;
                                            VendorOnBoarding.Remarks = VendorOnBoardingView.Remarks;
                                            VendorOnBoarding.MSME = VendorOnBoardingView.MSME;
                                            VendorOnBoarding.MSME_TYPE = VendorOnBoardingView.MSME_TYPE;
                                            VendorOnBoarding.MSME_Att_ID = VendorOnBoardingView.MSME_Att_ID;
                                            VendorOnBoarding.Reduced_TDS = VendorOnBoardingView.Reduced_TDS;
                                            VendorOnBoarding.TDS_RATE = VendorOnBoardingView.TDS_RATE;
                                            VendorOnBoarding.TDS_Att_ID = VendorOnBoardingView.TDS_Att_ID;
                                            VendorOnBoarding.RP = VendorOnBoardingView.RP;
                                            VendorOnBoarding.RP_Name = VendorOnBoardingView.RP_Name;
                                            VendorOnBoarding.RP_Type = VendorOnBoardingView.RP_Type;
                                            VendorOnBoarding.RP_Att_ID = VendorOnBoardingView.RP_Att_ID;
                                            //VendorOnBoarding.Field1 = VendorOnBoardingView.Field1;
                                            //VendorOnBoarding.Field2 = VendorOnBoardingView.Field2;
                                            //VendorOnBoarding.Field3 = VendorOnBoardingView.Field3;
                                            //VendorOnBoarding.Field4 = VendorOnBoardingView.Field4;
                                            //VendorOnBoarding.Field5 = VendorOnBoardingView.Field5;
                                            //VendorOnBoarding.Field6 = VendorOnBoardingView.Field6;
                                            //VendorOnBoarding.Field7 = VendorOnBoardingView.Field7;
                                            //VendorOnBoarding.Field8 = VendorOnBoardingView.Field8;
                                            //VendorOnBoarding.Field9 = VendorOnBoardingView.Field9;
                                            //VendorOnBoarding.Field10 = VendorOnBoardingView.Field10;
                                            VendorOnBoarding.IsActive = true;
                                            VendorOnBoarding.CreatedOn = DateTime.Now;
                                            var result = _dbContext.BPVendorOnBoardings.Add(VendorOnBoarding);
                                            await _dbContext.SaveChangesAsync();

                                            IdentityRepository identityRepository = new IdentityRepository(_dbContext);
                                            await identityRepository.CreateIdentities(VendorOnBoardingView.bPIdentities, result.Entity.TransID);

                                            BankRepository BankRepository = new BankRepository(_dbContext);
                                            await BankRepository.CreateBanks(VendorOnBoardingView.bPBanks, result.Entity.TransID);

                                            ContactRepository ContactRepository = new ContactRepository(_dbContext);
                                            await ContactRepository.CreateContacts(VendorOnBoardingView.bPContacts, result.Entity.TransID);

                                            //ActivityLogRepository ActivityLogRepository = new ActivityLogRepository(_dbContext);
                                            //await ActivityLogRepository.CreateActivityLogs(VendorOnBoardingView.bPActivityLogs, result.Entity.TransID);

                                            //VendorOnBoardingView.QuestionAnswers.ForEach(x => { x.AppUID = result.Entity.TransID; });
                                            //QuestionnaireRepository questionnaireRepository = new QuestionnaireRepository(_dbContext, _configuration);
                                            //await questionnaireRepository.SaveAnswers(VendorOnBoardingView.QuestionAnswers);

                                            // Updating TokenHistory
                                            history.UsedOn = DateTime.Now;
                                            history.IsUsed = true;
                                            history.Comment = "Token Used successfully";
                                            await _dbContext.SaveChangesAsync();

                                            transaction.Commit();
                                            transaction.Dispose();
                                            return result.Entity;
                                        }
                                        else
                                        {
                                            throw new Exception("Vendor with same email address has already exist.");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        transaction.Rollback();
                                        transaction.Dispose();
                                        throw ex;
                                    }

                                }
                                //});
                                //return vendorOnBoarding;
                            }
                            else
                            {
                                throw new Exception("Token might have already used or wrong token");
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLog.WriteToFile("Master/ChectTokenValidity/Exception:- Getting TokenHistory - " + ex.Message, ex);
                            throw new Exception("Token might have already used or wrong token");
                        }

                    }
                    else
                    {
                        throw new Exception("Invalid token!");
                    }
                }
                else
                {
                    throw new Exception("Invalid token!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        //public async Task<BPVendorOnBoarding> UpdateVendorOnBoarding(BPVendorOnBoarding VendorOnBoarding)
        //{
        //    try
        //    {
        //        var entity = _dbContext.Set<BPVendorOnBoarding>().FirstOrDefault(x => x.TransID == VendorOnBoarding.TransID);
        //        if (entity == null)
        //        {
        //            return entity;
        //        }
        //        //_dbContext.Entry(VendorOnBoarding).State = EntityState.Modified;
        //        entity.Name = VendorOnBoarding.Name;
        //        entity.Role = VendorOnBoarding.Role;
        //        entity.LegalName = VendorOnBoarding.LegalName;
        //        entity.AddressLine1 = VendorOnBoarding.AddressLine1;
        //        entity.AddressLine2 = VendorOnBoarding.AddressLine2;
        //        entity.City = VendorOnBoarding.City;
        //        entity.State = VendorOnBoarding.State;
        //        entity.Country = VendorOnBoarding.Country;
        //        entity.PinCode = VendorOnBoarding.PinCode;
        //        entity.Type = VendorOnBoarding.Type;
        //        entity.Phone1 = VendorOnBoarding.Phone1;
        //        entity.Phone2 = VendorOnBoarding.Phone2;
        //        entity.Email1 = VendorOnBoarding.Email1;
        //        entity.Email2 = VendorOnBoarding.Email2;
        //        entity.VendorCode = VendorOnBoarding.VendorCode;
        //        entity.ParentVendor = VendorOnBoarding.ParentVendor;
        //        entity.Status = VendorOnBoarding.Status;
        //        entity.ModifiedBy = VendorOnBoarding.ModifiedBy;
        //        entity.ModifiedOn = DateTime.Now;
        //        await _dbContext.SaveChangesAsync();
        //        return VendorOnBoarding;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public async Task<BPVendorOnBoarding> UpdateVendorOnBoarding(BPVendorOnBoardingView VendorOnBoardingView)
        {

            string[] decryptedArray = new string[3];
            string result2 = string.Empty;
            try
            {
                try
                {
                    result2 = Decrypt(VendorOnBoardingView.Token, true);
                }
                catch
                {
                    throw new Exception("Invalid token!");
                }
                if (result2.Contains('|') && result2.Split('|').Length == 3)
                {
                    decryptedArray = result2.Split('|');
                }
                else
                {
                    throw new Exception("Invalid token!");
                }

                if (decryptedArray.Length == 3)
                {
                    DateTime date = DateTime.Parse(decryptedArray[2].Replace('+', ' '));
                    if (DateTime.Now > date)// Convert.ToDateTime(decryptedarray[2]))
                    {
                        throw new Exception("token expired!");
                    }
                    var DecryptedUserID = decryptedArray[0];

                    var user = (from tb in _dbContext.BPVendorOnBoardings
                                where tb.TransID.ToString() == DecryptedUserID && tb.IsActive
                                select tb).FirstOrDefault();

                    if (user.Plant == decryptedArray[1] && VendorOnBoardingView.TransID == user.TransID)
                    {
                        try
                        {
                            TokenHistory history = _dbContext.TokenHistories.Where(x => x.TransID == user.TransID && !x.IsUsed && x.Token == VendorOnBoardingView.Token).Select(r => r).FirstOrDefault();
                            if (history != null)
                            {
                                //BPVendorOnBoarding vendorOnBoarding = new BPVendorOnBoarding();
                                //var strategy = _dbContext.Database.CreateExecutionStrategy();
                                //await strategy.ExecuteAsync(async () =>
                                //{
                                using (var transaction = _dbContext.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        var VendorOnBoarding = _dbContext.Set<BPVendorOnBoarding>().FirstOrDefault(x => x.TransID == VendorOnBoardingView.TransID);
                                        if (VendorOnBoarding == null)
                                        {
                                            return VendorOnBoarding;
                                        }
                                        //_dbContext.Entry(VendorOnBoarding).State = VendorOnBoardingState.Modified;
                                        VendorOnBoarding.Name = VendorOnBoardingView.Name;
                                        VendorOnBoarding.Role = VendorOnBoardingView.Role;
                                        VendorOnBoarding.LegalName = VendorOnBoardingView.LegalName;
                                        VendorOnBoarding.AddressLine1 = VendorOnBoardingView.AddressLine1;
                                        VendorOnBoarding.AddressLine2 = VendorOnBoardingView.AddressLine2;
                                        VendorOnBoarding.City = VendorOnBoardingView.City;
                                        VendorOnBoarding.State = VendorOnBoardingView.State;
                                        VendorOnBoarding.Country = VendorOnBoardingView.Country;
                                        VendorOnBoarding.PinCode = VendorOnBoardingView.PinCode;
                                        VendorOnBoarding.Type = VendorOnBoardingView.Type;
                                        VendorOnBoarding.Plant = VendorOnBoardingView.Plant;
                                        VendorOnBoarding.GSTNumber = VendorOnBoardingView.GSTNumber;
                                        VendorOnBoarding.GSTStatus = VendorOnBoardingView.GSTStatus;
                                        VendorOnBoarding.PANNumber = VendorOnBoardingView.PANNumber;
                                        VendorOnBoarding.TypeofIndustry = VendorOnBoardingView.TypeofIndustry;
                                        VendorOnBoarding.Phone1 = VendorOnBoardingView.Phone1;
                                        VendorOnBoarding.Phone2 = VendorOnBoardingView.Phone2;
                                        VendorOnBoarding.Email1 = VendorOnBoardingView.Email1;
                                        // VendorOnBoarding.ContactPerson = VendorOnBoardingView.ContactPerson;
                                        VendorOnBoarding.Email2 = VendorOnBoardingView.Email2;
                                        //VendorOnBoarding.Invoice = VendorOnBoardingView.Invoice;
                                        //VendorOnBoarding.WebsiteAddress = VendorOnBoardingView.WebsiteAddress;
                                        //VendorOnBoarding.VendorCode = VendorOnBoardingView.VendorCode;
                                        //VendorOnBoarding.ParentVendor = VendorOnBoardingView.ParentVendor;
                                        VendorOnBoarding.Status = VendorOnBoardingView.Status;
                                        VendorOnBoarding.Department = VendorOnBoardingView.Department;
                                        VendorOnBoarding.AccountGroup = VendorOnBoardingView.AccountGroup;
                                        VendorOnBoarding.PurchaseOrg = VendorOnBoardingView.PurchaseOrg;
                                        VendorOnBoarding.CompanyCode = VendorOnBoardingView.CompanyCode;
                                        VendorOnBoarding.EmamiContactPerson = VendorOnBoardingView.EmamiContactPerson;
                                        VendorOnBoarding.EmamiContactPersonMail = VendorOnBoardingView.EmamiContactPersonMail;
                                        // VendorOnBoarding.ContactPerson = VendorOnBoardingView.ContactPerson;
                                        VendorOnBoarding.Remarks = VendorOnBoardingView.Remarks;
                                        VendorOnBoarding.MSME = VendorOnBoardingView.MSME;
                                        VendorOnBoarding.MSME_TYPE = VendorOnBoardingView.MSME_TYPE;
                                        VendorOnBoarding.MSME_Att_ID = VendorOnBoardingView.MSME_Att_ID;
                                        VendorOnBoarding.Reduced_TDS = VendorOnBoardingView.Reduced_TDS;
                                        VendorOnBoarding.TDS_RATE = VendorOnBoardingView.TDS_RATE;
                                        VendorOnBoarding.TDS_Att_ID = VendorOnBoardingView.TDS_Att_ID;
                                        VendorOnBoarding.RP = VendorOnBoardingView.RP;
                                        VendorOnBoarding.RP_Name = VendorOnBoardingView.RP_Name;
                                        VendorOnBoarding.RP_Type = VendorOnBoardingView.RP_Type;
                                        VendorOnBoarding.RP_Att_ID = VendorOnBoardingView.RP_Att_ID;
                                        VendorOnBoarding.IsActive = true;
                                        //VendorOnBoarding.CreatedOn = DateTime.Now;
                                        VendorOnBoarding.ModifiedBy = VendorOnBoardingView.ModifiedBy;
                                        VendorOnBoarding.ModifiedOn = DateTime.Now;
                                        await _dbContext.SaveChangesAsync();

                                        IdentityRepository identityRepository = new IdentityRepository(_dbContext);
                                        await identityRepository.DeleteIdentityByTransID(VendorOnBoardingView.TransID);
                                        await identityRepository.CreateIdentities(VendorOnBoardingView.bPIdentities, VendorOnBoardingView.TransID);

                                        BankRepository BankRepository = new BankRepository(_dbContext);
                                        await BankRepository.DeleteBankByTransID(VendorOnBoardingView.TransID);
                                        await BankRepository.CreateBanks(VendorOnBoardingView.bPBanks, VendorOnBoardingView.TransID);

                                        ContactRepository ContactRepository = new ContactRepository(_dbContext);
                                        await ContactRepository.DeleteContactByTransID(VendorOnBoardingView.TransID);
                                        await ContactRepository.CreateContacts(VendorOnBoardingView.bPContacts, VendorOnBoardingView.TransID);

                                        //ActivityLogRepository ActivityLogRepository = new ActivityLogRepository(_dbContext);
                                        //await ActivityLogRepository.DeleteActivityLogByTransID(VendorOnBoardingView.TransID);
                                        //await ActivityLogRepository.CreateActivityLogs(VendorOnBoardingView.bPActivityLogs, VendorOnBoardingView.TransID);

                                        //VendorOnBoardingView.QuestionAnswers.ForEach(x => { x.AppUID = VendorOnBoardingView.TransID; });
                                        //QuestionnaireRepository questionnaireRepository = new QuestionnaireRepository(_dbContext, _configuration);
                                        //await questionnaireRepository.SaveAnswers(VendorOnBoardingView.QuestionAnswers);

                                        if (VendorOnBoarding.Status.ToLower() == "registered")
                                        {
                                            // Updating TokenHistory
                                            history.UsedOn = DateTime.Now;
                                            history.IsUsed = true;
                                            history.Comment = "Token Used successfully";
                                            await _dbContext.SaveChangesAsync();

                                            var result = await CreateVendorUser(VendorOnBoardingView);
                                            if (result != null)
                                            {
                                                transaction.Commit();
                                                transaction.Dispose();
                                            }
                                            else
                                            {
                                                transaction.Rollback();
                                                transaction.Dispose();
                                                return VendorOnBoarding;
                                            }
                                        }

                                        return VendorOnBoarding;
                                    }
                                    catch (Exception ex)
                                    {
                                        transaction.Rollback();
                                        transaction.Dispose();
                                        throw ex;
                                    }
                                }
                                //});
                                //return vendorOnBoarding;
                            }
                            else
                            {
                                throw new Exception("Token might have already used or wrong token");
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLog.WriteToFile("Master/ChectTokenValidity/Exception:- Getting TokenHistory - " + ex.Message, ex);
                            throw new Exception("Token might have already used or wrong token");
                        }

                    }
                    else
                    {
                        throw new Exception("Invalid token!");
                    }
                }
                else
                {
                    throw new Exception("Invalid token!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<BPVendorOnBoardingView> CreateVendorUser(BPVendorOnBoardingView VendorOnBoardingView)
        {
            try
            {
                WriteLog.WriteToFile($"FactRepository/CreateVendorUser:- Create vendor user called for {VendorOnBoardingView.LegalName}");
                string BaseAddress = _configuration.GetValue<string>("APIBaseAddress");
                string HostURI = BaseAddress + "/authenticationapi/Master/SendMailToApprover";
                var uri = new Uri(HostURI);
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                var SerializedObject = JsonConvert.SerializeObject(VendorOnBoardingView);
                byte[] requestBody = Encoding.UTF8.GetBytes(SerializedObject);

                using (var postStream = await request.GetRequestStreamAsync())
                {
                    await postStream.WriteAsync(requestBody, 0, requestBody.Length);
                }

                try
                {
                    using (var response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        if (response != null && response.StatusCode == HttpStatusCode.OK)
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            return VendorOnBoardingView;
                        }
                        else
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            return null;
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var errorMessage = reader.ReadToEnd();
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPVendorOnBoarding> DeleteVendorOnBoarding(BPVendorOnBoarding VendorOnBoarding)
        {
            try
            {
                //var entity = await _dbContext.Set<BPVendorOnBoarding>().FindAsync(VendorOnBoarding.VendorOnBoarding, VendorOnBoarding.Language);
                var entity = _dbContext.Set<BPVendorOnBoarding>().FirstOrDefault(x => x.TransID == VendorOnBoarding.TransID);
                if (entity == null)
                {
                    return entity;
                }

                _dbContext.Set<BPVendorOnBoarding>().Remove(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPVendorOnBoarding> ApproveVendor(BPVendorOnBoarding VendorOnBoarding)
        {
            try
            {
                var entity = _dbContext.Set<BPVendorOnBoarding>().FirstOrDefault(x => x.TransID == VendorOnBoarding.TransID);
                if (entity == null)
                {
                    return entity;
                }
                entity.Status = "Approved";
                entity.ModifiedBy = VendorOnBoarding.ModifiedBy;
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                //await SendMailToApprovalVendor(entity.Email1, entity.Phone1);
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPVendorOnBoarding> GetDeclarationID(int TransID)
        {
            var result = (from tb in _dbContext.BPVendorOnBoardings
                          where tb.TransID == TransID
                          select new BPVendorOnBoarding()
                          {
                              MSME_Att_ID = tb.MSME_Att_ID,
                              TDS_Att_ID = tb.TDS_Att_ID,
                              RP_Att_ID = tb.RP_Att_ID
                          }).ToList();
            return result;
        }
        public BPVendorOnBoarding GetAttachmentId(string arg1)
        {
            //_dbContext.Set<BPVendorOnBoarding>().FirstOrDefault(x => x.TransID == VendorOnBoarding.TransID);
            var result = (from tb in _dbContext.BPAttachments
                          where tb.AttachmentID.ToString() == arg1
                          select new BPVendorOnBoarding()
                          {
                              MSME_Att_ID = tb.AttachmentID.ToString()
                          }).FirstOrDefault();
            return result;
        }
        public BPAttachment GetBPAttachmentByAttachmentId(int attachAttachmentId)
        {
            try
            {
                var result = _dbContext.BPAttachments.FirstOrDefault(x => x.AttachmentID == attachAttachmentId);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteVendorOnboardingById(string Transid)
        {
            try
            {
                var VOBresult = _dbContext.BPVendorOnBoardings.Where(x => x.TransID == int.Parse(Transid)).FirstOrDefault();
                if (VOBresult != null)
                {
                    _dbContext.BPVendorOnBoardings.Remove(VOBresult);
                }
                var BANKresult = _dbContext.BPBanks.Where(x => x.TransID == int.Parse(Transid)).FirstOrDefault();
                if (BANKresult != null)
                {
                    _dbContext.BPBanks.Remove(BANKresult);
                }
                var Contactresult = _dbContext.BPContacts.Where(x => x.TransID == int.Parse(Transid)).FirstOrDefault();
                if (Contactresult != null)
                {
                    _dbContext.BPContacts.Remove(Contactresult);
                }
                var IDresult = _dbContext.BPIdentities.Where(x => x.TransID == int.Parse(Transid)).FirstOrDefault();
                if (IDresult != null)
                {
                    _dbContext.BPIdentities.Remove(IDresult);
                }
                var DocResult = _dbContext.BPAttachments.Where(x => x.HeaderNumber == Transid).ToList();
                if (DocResult != null)
                {
                    _dbContext.BPAttachments.RemoveRange(DocResult);
                }
                await _dbContext.SaveChangesAsync();
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPVendorOnBoarding> GetDeclaration_toogle(int TransID)
        {
            var result = (from tb in _dbContext.BPVendorOnBoardings
                          where tb.TransID == TransID
                          select new BPVendorOnBoarding()
                          {
                              MSME = tb.MSME,
                              RP = tb.RP,
                              Reduced_TDS = tb.Reduced_TDS
                          }).ToList();
            return result;
        }

        //public List<BPVendorOnBoarding> GetDeclarationIDWithAttachment(int DeclarationID)
        //{
        //    int MSME_Att_ID=DeclarationID[0].
        //    //var result =(from tb1  in _dbContext.BPAttachments where tb1.AttachmentID==DeclarationID. )
        //}




        //public List<BPVendorOnBoarding> GetAllVendorOnBoardings()
        //{
        //    try
        //    {
        //        return _dbContext.BPVendorOnBoardings.ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public async Task<BPVendorOnBoarding> RejectVendor(BPVendorOnBoarding VendorOnBoarding)
        {
            try
            {
                var entity = _dbContext.Set<BPVendorOnBoarding>().FirstOrDefault(x => x.TransID == VendorOnBoarding.TransID);
                if (entity == null)
                {
                    return entity;
                }
                entity.Status = "Rejected";
                entity.ModifiedBy = VendorOnBoarding.ModifiedBy;
                entity.ModifiedOn = DateTime.Now;

                entity.Remarks = VendorOnBoarding.Remarks;
                var Remark = VendorOnBoarding.Remarks;
                await _dbContext.SaveChangesAsync();
                await SendMailToRejectVendor(entity.Email1, entity.Phone1, Remark);
                return VendorOnBoarding;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        //private Task SendMailToRejectVendor(object email, object phone)
        //{
        //    throw new NotImplementedException();
        //}

        #region Question 

        public QuestionnaireResultSet GetQuestionnaireResultSetByQRID()
        {
            try
            {
                string BaseAddress = _configuration.GetValue<string>("QuestionnaireBaseAddress");
                string QRID = "1";
                Guid userId = default(Guid);
                QuestionnaireResultSet questionnaireResultSet = null;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(BaseAddress + "api/Questionnaire/GetQuestionnaireResultSetByQRID?QRID=" + QRID + "&" + userId);
                request.Method = "GET";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    string jsonRes = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    questionnaireResultSet = JsonConvert.DeserializeObject<QuestionnaireResultSet>(jsonRes);
                    reader.Close();
                    dataStream.Close();
                }
                return questionnaireResultSet;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        //public static bool InsertInvoiceData(InsertInvoiceDetail insertInvoiceDetail)
        //{
        //    bool status = false;
        //    try
        //    {
        //        using (var client = new WebClient())
        //        {
        //            client.Headers.Add("Content-Type:application/json");
        //            client.Headers.Add("Accept:application/json");
        //            var result1 = client.UploadString(BaseAddress + "api/Invoice/InsertInvoiceDetails/", "Post", JsonConvert.SerializeObject(insertInvoiceDetail));
        //            Log.WriteLog("InsertInvoiceData:- Invoice Data is inserted into Db from Xml");
        //            InsertInvoiceResponse insertInvoiceResponse = JsonConvert.DeserializeObject<InsertInvoiceResponse>(result1);
        //            if (insertInvoiceResponse.Status)
        //            {
        //                status = true;
        //            }
        //            else
        //            {
        //                status = false;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        status = false;
        //        Log.WriteLog("InsertInvoiceData/Exception:- " + ex.Message);
        //    }
        //    return status;
        //}

        #endregion

        //ExalcaSMTP
        //public async Task<bool> SendMail(string code, string UserName, string toEmail, string TransID, string siteURL)
        //{
        //    try
        //    {
        //        var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
        //        string hostName = STMPDetailsConfig["Host"];
        //        string SMTPEmail = STMPDetailsConfig["Email"];
        //        string SMTPEmailPassword = STMPDetailsConfig["Password"];
        //        string SMTPPort = STMPDetailsConfig["Port"];
        //        var message = new MailMessage();
        //        string subject = "";
        //        StringBuilder sb = new StringBuilder();
        //        //string UserName = _ctx.TBL_User_Master.Where(x => x.Email == toEmail).Select(y => y.UserName).FirstOrDefault();
        //        //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
        //        sb.Append(string.Format("Dear {0},<br/>", UserName));
        //        sb.Append("You have invited to register in our business process by Wipro, Request you to proceed with registration");
        //        sb.Append("<p><a href=\"" + siteURL + "/#/register/vendor?token=" + code + "&Id=" + TransID + "&Email=" + toEmail + "\"" + ">Register</a></p>");
        //        sb.Append($"<i>Note: The verification link will expire in {_tokenTimespan} days.<i>");
        //        sb.Append("<p>Regards,</p><p>Admin</p>");
        //        subject = "Vendor Registration Initialization";
        //        SmtpClient client = new SmtpClient();
        //        client.Port = Convert.ToInt32(SMTPPort);
        //        client.Host = hostName;
        //        client.EnableSsl = true;
        //        client.Timeout = 60000;
        //        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        client.UseDefaultCredentials = false;
        //        client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);
        //        MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
        //        reportEmail.BodyEncoding = UTF8Encoding.UTF8;
        //        reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
        //        reportEmail.IsBodyHtml = true;
        //        //ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        //        await client.SendMailAsync(reportEmail);
        //        WriteLog.WriteToFile($"Registration link has been sent successfully to {toEmail}");
        //        return true;
        //    }
        //    catch (SmtpFailedRecipientsException ex)
        //    {
        //        for (int i = 0; i < ex.InnerExceptions.Length; i++)
        //        {
        //            SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
        //            if (status == SmtpStatusCode.MailboxBusy ||
        //                status == SmtpStatusCode.MailboxUnavailable)
        //            {
        //                WriteLog.WriteToFile("VendorOnBoardingRepository/SendMail/MailboxBusy/MailboxUnavailable/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
        //            }
        //            else
        //            {
        //                WriteLog.WriteToFile("VendorOnBoardingRepository/SendMail/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
        //            }
        //        }
        //        WriteLog.WriteToFile("VendorOnBoardingRepository/SendMail/SmtpFailedRecipientsException:- " + ex.Message, ex);
        //        return false;
        //    }
        //    catch (SmtpException ex)
        //    {
        //        WriteLog.WriteToFile("VendorOnBoardingRepository/SendMail/SmtpException:- " + ex.Message, ex);
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.WriteToFile("VendorOnBoardingRepository/SendMail/Exception:- " + ex.Message, ex);
        //        return false;
        //    }
        //}

        //Wipro SMTP
        public async Task<bool> SendMail(string code, string UserName, string toEmail, string TransID, string siteURL)
        {
            try
            {
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];
                var message = new MailMessage();
                string subject = "";
                StringBuilder sb = new StringBuilder();
                UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
              
                sb.Append(@"<html><head></head><body> <div style='border:1px solid #dbdbdb;'> <div style='padding: 20px 20px; background-color: #fff06769;text-align: center;font-family: Segoe UI;'> <p> <h2>Wipro Vendor Onboarding</h2> </p> </div> <div style='background-color: #f8f7f7;padding: 20px 20px;font-family: Segoe UI'> <div style='padding: 20px 20px;border:1px solid white;background-color: white !important'> <p>Dear concern,</p> <p>You have invited to register in our business process by Wipro, Request you to proceed with registration.</p> <div style='text-align: end;'>" + "<a href=\"" + siteURL + "/#/register/vendor?token=" + code + "&Id=" + TransID + "&Email=" + toEmail + "\"" + "><button style='width: 90px;height: 28px; background-color: #039be5;color: white'>Register</button></a></div> <p>Note: The verification link will expire in " + _tokenTimespan + " days.</p> <p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>");
                //sb.Append(string.Format("Dear {0},<br/>", UserName));
                //sb.Append("You have invited to register in our business process by Wipro, Request you to proceed with registration");
                //sb.Append("<p><a href=\"" + siteURL + "/#/register/vendor?token=" + code + "&Id=" + TransID + "&Email=" + toEmail + "\"" + ">Register</a></p>");
                //sb.Append($"<i>Note: The verification link will expire in {_tokenTimespan} days.<i>");
                //sb.Append("<p>Regards,</p><p>Admin</p>");

                //sb.Append(@"<html><head></head><body><div style='border:1px solid #dbdbdb;'><div style='padding: 20px 20px; background-color: #fff06769;text-align: center;font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;'><p><h2>Wipro Vendor Onboadring</h2></p></div><div style='background-color: #f8f7f7;padding: 20px 20px;font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;'><div style='padding: 20px 20px;border:1px solid white;background-color: white !important;'><p>Dear concern</p><p>You have invited to register in our business process by Wipro, Request you to proceed with registration</p><div style='text-align: end;'>" + "<a href =\"" + siteURL + "/#/register/vendor?token=" + code + "&Id=" + TransID + "&Email=" + toEmail + "\"" + "><button style='width: 90px;height: 28px;backgroud-color:red;background-color: #008CBA;color: white'>Register</button></a>" + "</div></div></div></div></body></html>");

                //sb.Append(@"<html><head></head><body> <div style='border:1px solid #dbdbdb;'> <div style='padding: 20px 20px; background-color: #fff06769;text-align: center;font-family: Segoe UI;'> <p> <h2>Wipro Vendor Onboadring</h2> </p> </div> <div style='background-color: #f8f7f7;padding: 20px 20px;font-family: Segoe UI'> <div style='padding: 20px 20px;border:1px solid white;background-color: white !important'> <p>Dear concern,</p> <p>You have invited to register in our business process by Wipro, Request you to proceed with registration.</p> <div style='text-align: end;'>" + "<a href=\"" + siteURL + "/#/register/vendor?token=" + code + "&Id=" + TransID + "&Email=" + toEmail + "\"" + "><button style='width: 90px;height: 28px; background-color: #039be5;color: white'>Register</button> </div> <p>Note: The verification link will expire in " + _tokenTimespan + " days.</p> <p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>");
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
                WriteLog.WriteToFile($"Registration link has been sent successfully to {toEmail}");
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
                        WriteLog.WriteToFile("VendorOnBoardingRepository/SendMail/MailboxBusy/MailboxUnavailable/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                    else
                    {
                        WriteLog.WriteToFile("VendorOnBoardingRepository/SendMail/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                }
                WriteLog.WriteToFile("VendorOnBoardingRepository/SendMail/SmtpFailedRecipientsException:- " + ex.Message, ex);
                return false;
            }
            catch (SmtpException ex)
            {
                WriteLog.WriteToFile("VendorOnBoardingRepository/SendMail/SmtpException:- " + ex.Message, ex);
                return false;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("VendorOnBoardingRepository/SendMail/Exception:- " + ex.Message, ex);
                return false;
            }
        }

        public async Task<bool> SendMailToInitializedVendor(string toEmail, string password)
        {
            try
            {
                //string hostName = ConfigurationManager.AppSettings["HostName"];
                //string SMTPEmail = ConfigurationManager.AppSettings["SMTPEmail"];
                ////string fromEmail = ConfigurationManager.AppSettings["FromEmail"];
                //string SMTPEmailPassword = ConfigurationManager.AppSettings["SMTPEmailPassword"];
                //string SMTPPort = ConfigurationManager.AppSettings["SMTPPort"];
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                string siteURL = _configuration["PortalAddress"];
                string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];
                var message = new MailMessage();
                string subject = "";
                StringBuilder sb = new StringBuilder();
                //string UserName = _dbContext.TBL_User_Master.Where(x => x.Email == toEmail).Select(y => y.UserName).FirstOrDefault();
                //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
                sb.Append(string.Format("Dear {0},<br/>", toEmail));
                sb.Append("<p>Your  Registration is Approved.</p>");
                sb.Append("<p>Please Login by clicking <a href=\"" + siteURL + "/#/auth/login\">here</a></p>");
                sb.Append(string.Format("<p>User name: {0}</p>", toEmail));
                sb.Append(string.Format("<p>Password: {0}</p>", password));
                sb.Append("<p>Regards,</p><p>Admin</p>");
                subject = "BP Cloud Vendor Registration";
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = false;
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);
                //client.Credentials = new System.Net.NetworkCredential(SMTPEmail.Trim(), SMTPEmailPassword.Trim());
                MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                await client.SendMailAsync(reportEmail);
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/SendMail : - ", ex);
                throw ex;
            }
        }
        public async Task<bool> SendMailToApprovalVendor(string toEmail, string password)
        {
            try
            {
                //string hostName = ConfigurationManager.AppSettings["HostName"];
                //string SMTPEmail = ConfigurationManager.AppSettings["SMTPEmail"];
                ////string fromEmail = ConfigurationManager.AppSettings["FromEmail"];
                //string SMTPEmailPassword = ConfigurationManager.AppSettings["SMTPEmailPassword"];
                //string SMTPPort = ConfigurationManager.AppSettings["SMTPPort"];
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                string siteURL = _configuration["PortalAddress"];
                string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];
                var message = new MailMessage();
                string subject = "";
                StringBuilder sb = new StringBuilder();
                //string UserName = _dbContext.TBL_User_Master.Where(x => x.Email == toEmail).Select(y => y.UserName).FirstOrDefault();
                //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
                sb.Append(string.Format("Dear {0},<br/>", toEmail));
                sb.Append("<p>Vendor Registred.</p>");
                //sb.Append("<p>Please Login by clicking <a href=\"" + siteURL + "/#/auth/login\">here</a></p>");
                //sb.Append(string.Format("<p>User name: {0}</p>", toEmail));
                //sb.Append(string.Format("<p>Password: {0}</p>", password));
                sb.Append("<p>Regards,</p><p>Admin</p>");
                subject = "BP Cloud Vendor Registration";
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = false;
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);
                MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                await client.SendMailAsync(reportEmail);
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/SendMail : - ", ex);
                throw ex;
            }
        }

        public async Task<bool> SendMailToRejectVendor(string toEmail, string password, string Remarks)
        {
            try
            {
                //string hostName = ConfigurationManager.AppSettings["HostName"];
                //string SMTPEmail = ConfigurationManager.AppSettings["SMTPEmail"];
                ////string fromEmail = ConfigurationManager.AppSettings["FromEmail"];
                //string SMTPEmailPassword = ConfigurationManager.AppSettings["SMTPEmailPassword"];
                //string SMTPPort = ConfigurationManager.AppSettings["SMTPPort"];
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                string siteURL = _configuration["PortalAddress"];
                string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];
                var message = new MailMessage();
                string subject = "";
                StringBuilder sb = new StringBuilder();
                //string UserName = _dbContext.TBL_User_Master.Where(x => x.Email == toEmail).Select(y => y.UserName).FirstOrDefault();
                //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
                sb.Append(string.Format("Dear {0},<br/>", toEmail));
                sb.Append("<p>Your Registration is Rejected.</p>");
                sb.Append(string.Format("<p>Remarks:{0}<p>", Remarks));
                //sb.Append("<p>Please Login by clicking <a href=\"" + siteURL + "/#/auth/login\">here</a></p>");
                //sb.Append(string.Format("<p>User name: {0}</p>", toEmail));
                //sb.Append(string.Format("<p>Password: {0}</p>", password));
                sb.Append("<p>Regards,</p><p>Admin</p>");
                subject = "BP Cloud Vendor Registration";
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = false;
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);
                MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                await client.SendMailAsync(reportEmail);
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/SendMail : - ", ex);
                throw ex;
            }
        }

        private class ErrorLog
        {
            internal static void WriteToFile(string v, Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        #region EncryptAndDecrypt

        public string Encrypt(string Password, bool useHashing)
        {
            try
            {
                string EncryptionKey = _configuration["EncryptionKey"];
                byte[] KeyArray;
                byte[] ToEncryptArray = UTF8Encoding.UTF8.GetBytes(Password);
                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    KeyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(EncryptionKey));
                    hashmd5.Clear();
                }
                else
                    KeyArray = UTF8Encoding.UTF8.GetBytes(EncryptionKey);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = KeyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] resultArray =
                  cTransform.TransformFinalBlock(ToEncryptArray, 0,
                  ToEncryptArray.Length);

                tdes.Clear();
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/Encrypt/Exception:- " + ex.Message, ex);
                return null;
            }
        }

        public string Decrypt(string Password, bool UseHashing)
        {
            try
            {
                string EncryptionKey = _configuration["EncryptionKey"];
                byte[] KeyArray;
                byte[] ToEncryptArray = Convert.FromBase64String(Password);
                if (UseHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    KeyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(EncryptionKey));
                    hashmd5.Clear();
                }
                else
                {
                    KeyArray = UTF8Encoding.UTF8.GetBytes(EncryptionKey);
                }

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = KeyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(
                                     ToEncryptArray, 0, ToEncryptArray.Length);
                tdes.Clear();
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/Decrypt/Exception:- " + ex.Message, ex);
                return null;
            }
        }

        public async Task<BPVendorOnBoarding> UpdateVendorOnBoardingStatus(BPVendorOnBoarding vendor, string status)
        {
            try
            {
                var entity = _dbContext.Set<BPVendorOnBoarding>().FirstOrDefault(x => x.TransID == vendor.TransID);
                if (entity == null)
                {
                    return entity;
                }
                entity.Status = status;
                entity.ModifiedBy = vendor.ModifiedBy;
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                //await SendMailToApprovalVendor(entity.Email1, entity.Phone1);
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
