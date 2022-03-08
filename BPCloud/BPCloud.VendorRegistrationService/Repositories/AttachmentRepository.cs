using BPCloud.VendorRegistrationService.DBContexts;
using BPCloud.VendorRegistrationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorRegistrationService.Repositories
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly RegistrationContext _dbContext;

        public AttachmentRepository(RegistrationContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task DeleteAttachment(string AppNumber, string HeaderNumber)
        {
            try
            {
                var BPAttachment = _dbContext.BPAttachments.Where(x => x.AppNumber == AppNumber && x.HeaderNumber == HeaderNumber).ToList();
                BPAttachment.ForEach(x => _dbContext.BPAttachments.Remove(x));
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BPAttachment> AddAttachment(BPAttachment BPAttachment)
        {
            try
            {
                var BPAttachment1 = _dbContext.BPAttachments.Where(x => x.AppNumber == BPAttachment.AppNumber && x.HeaderNumber == BPAttachment.HeaderNumber
                && x.AttachmentName != BPAttachment.AttachmentName).ToList();
                BPAttachment1.ForEach(x => _dbContext.BPAttachments.Remove(x));
                await _dbContext.SaveChangesAsync();
                BPAttachment bPAttachment = _dbContext.BPAttachments.FirstOrDefault(x => x.HeaderNumber == BPAttachment.HeaderNumber && x.AttachmentName == BPAttachment.AttachmentName);
                if (bPAttachment == null)
                {
                    BPAttachment.IsActive = true;
                    BPAttachment.CreatedOn = DateTime.Now;
                    var result = _dbContext.BPAttachments.Add(BPAttachment);
                    await _dbContext.SaveChangesAsync();
                }
                return BPAttachment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPAttachment> UpdateAttachment(BPAttachment BPAttachment)
        {
            try
            {
                BPAttachment bPAttachment = _dbContext.BPAttachments.FirstOrDefault(x => x.HeaderNumber == BPAttachment.HeaderNumber && x.AttachmentName == BPAttachment.AttachmentName);
                if (bPAttachment != null)
                {
                    bPAttachment.AttachmentFile = BPAttachment.AttachmentFile;
                    bPAttachment.ContentLength = BPAttachment.ContentLength;
                    bPAttachment.ContentType = BPAttachment.ContentType;
                    bPAttachment.IsActive = true;
                    bPAttachment.CreatedOn = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BPAttachment> UploadUserAttachment(BPAttachment BPAttachment,string perviousName)
        {
            try
            {
                if(perviousName  != "null")
                {
                    BPAttachment perviousAttachment = _dbContext.BPAttachments.FirstOrDefault(x => x.HeaderNumber == BPAttachment.HeaderNumber && x.AttachmentName == perviousName);

                    if(perviousAttachment != null)
                    {
                        BPAttachment bPAttachment = _dbContext.BPAttachments.FirstOrDefault(x => x.HeaderNumber == BPAttachment.HeaderNumber && x.AttachmentName == BPAttachment.AttachmentName);
                        if(bPAttachment == null)
                        {
                            perviousAttachment.AttachmentFile = BPAttachment.AttachmentFile;
                            perviousAttachment.AttachmentName = BPAttachment.AttachmentName;
                            perviousAttachment.ContentType = BPAttachment.ContentType;
                            perviousAttachment.ContentLength = BPAttachment.ContentLength;
                            await _dbContext.SaveChangesAsync();
                            return perviousAttachment;
                        }
                        return null;
                    }
                }
                else
                {
                    BPAttachment bPAttachment = _dbContext.BPAttachments.FirstOrDefault(x => x.HeaderNumber == BPAttachment.HeaderNumber && x.AttachmentName == BPAttachment.AttachmentName);
                    if (bPAttachment == null)
                    {
                        BPAttachment.IsActive = true;
                        BPAttachment.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPAttachments.Add(BPAttachment);
                        await _dbContext.SaveChangesAsync();
                        return BPAttachment;
                    }
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPAttachment> FilterAttachments(string ProjectName, int AppID = 0, string AppNumber = null)
        {
            try
            {
                bool IsAppID = AppID > 0;
                bool IsAppNumber = !string.IsNullOrEmpty(AppNumber);
                var result = (from tb in _dbContext.BPAttachments
                              where tb.ProjectName == ProjectName && (!IsAppID || tb.AppID == AppID) &&
                              (!IsAppNumber || tb.AppNumber == AppNumber) && tb.IsActive
                              select tb).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BPAttachment FilterAttachment(string ProjectName, string AttchmentName, int AppID = 0, string AppNumber = null, string HeaderNumber = null)
        {
            try
            {
                bool IsAppID = AppID > 0;
                bool IsAppNumber = !string.IsNullOrEmpty(AppNumber);
                bool IsHeaderNumber = !string.IsNullOrEmpty(HeaderNumber);
                var result = (from tb in _dbContext.BPAttachments
                              where tb.ProjectName == ProjectName && tb.AttachmentName == AttchmentName && (!IsAppID || tb.AppID == AppID) &&
                              (!IsAppNumber || tb.AppNumber == AppNumber) && (!IsHeaderNumber || tb.HeaderNumber == HeaderNumber) && tb.IsActive
                              select tb).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BPAttachment GetAttachmentByName(string AttachmentName)
        {
            try
            {
                var BPAttachment = _dbContext.BPAttachments.FirstOrDefault(x => x.AttachmentName == AttachmentName);
                return BPAttachment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BPAttachment GetAttachmentByID(int AttachmentID)
        {
            try
            {
                var BPAttachment = _dbContext.BPAttachments.FirstOrDefault(x => x.AttachmentID == AttachmentID);
                return BPAttachment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public BPAttachment GetIdentityAttachment(string AppNumber, string HeaderNumber, string AttachmentName)
        {
            try
            {
                var BPAttachment = _dbContext.BPAttachments.FirstOrDefault(x => x.AppNumber == AppNumber && x.HeaderNumber == HeaderNumber && x.AttachmentName == AttachmentName);
                return BPAttachment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async void UpdateDecelrationId(BPAttachment attachmentData, string TransID, string Name)
        {
            try
            {


                if (Name == "MSME")
                {
                    BPVendorOnBoarding VPVendor = _dbContext.BPVendorOnBoardings.Where(x => x.TransID == int.Parse(TransID)).FirstOrDefault();
                    VPVendor.MSME_Att_ID = attachmentData.AttachmentID.ToString();
                    //await _dbContext.SaveChangesAsync();
                }
                else if (Name == "RP")
                {
                    BPVendorOnBoarding VPVendor = _dbContext.BPVendorOnBoardings.Where(x => x.TransID == int.Parse(TransID)).FirstOrDefault();
                    VPVendor.RP_Att_ID = attachmentData.AttachmentID.ToString();
                    //await _dbContext.SaveChangesAsync();
                }
                else if (Name == "LTDS")
                {
                    BPVendorOnBoarding VPVendor = _dbContext.BPVendorOnBoardings.Where(x => x.TransID == int.Parse(TransID)).FirstOrDefault();
                    VPVendor.TDS_Att_ID = attachmentData.AttachmentID.ToString();
                    //await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Decleration Name is not found");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BPAttachment> AddDeclerationAttachment(BPAttachment attachmentData, string TransID, string Name)
        {
            var attachment=false;
            try
            {
                var result = _dbContext.BPAttachments.Where(x => x.HeaderNumber == TransID && x.AttachmentName == attachmentData.AttachmentName).FirstOrDefault();
                if (result != null)
                {

                    UpdateDecelrationId(result, TransID, Name);
                    attachmentData = null;
                }
                else
                {
                    var Attachmentresult = _dbContext.BPAttachments.Where(x => x.HeaderNumber == TransID && x.AppNumber == Name).FirstOrDefault();

                    if(Attachmentresult != null)
                    {
                        _dbContext.BPAttachments.Remove(Attachmentresult);
                    }
                    attachmentData.ProjectName = "BPCloud";
                    attachmentData.AppID = 1;
                    attachmentData.IsHeaderExist = true;
                    attachmentData.HeaderNumber = TransID;
                    attachmentData.AppNumber = Name;
                    var AttachmentResult = _dbContext.BPAttachments.Add(attachmentData);
                    attachment = true;
                }
                await _dbContext.SaveChangesAsync();
                var data = attachmentData;
                return data;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BPAttachment GetAttachmentByIDAndName(string HeaderNumber, string AttachmentName)
        {
            try
            {
                var BPAttachment = _dbContext.BPAttachments.FirstOrDefault(x => x.HeaderNumber == HeaderNumber &&x.AttachmentName == AttachmentName);
                return BPAttachment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
