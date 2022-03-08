using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BPCloud.VendorRegistrationService.DBContexts;
using BPCloud.VendorRegistrationService.Models;
using BPCloud.VendorRegistrationService.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPCloud.VendorRegistrationService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AttachmentController : ControllerBase
    {
        private readonly IAttachmentRepository _AttachmentRepository;
        private readonly RegistrationContext _dbContext;

        public AttachmentController(IAttachmentRepository attachmentRepository, RegistrationContext dbContext)
        {
            _AttachmentRepository = attachmentRepository;
            _dbContext = dbContext;

        }

        [HttpPost]
        public async Task<IActionResult> AddUserAttachment()
        {
            try
            {
                var request = Request;
                var TransID = request.Form["TransID"].ToString();
                var CreatedBy = request.Form["CreatedBy"].ToString();
                var PerviousFileName = request.Form["PerviousFileName"].ToString();
                IFormFileCollection postedfiles = request.Form.Files;

                if (postedfiles.Count > 0)
                {
                    for (int i = 0; i < postedfiles.Count; i++)
                    {
                        var FileName = postedfiles[i].FileName;
                        var ContentType = postedfiles[i].ContentType;
                        var ContentLength = postedfiles[i].Length;
                        using (Stream st = postedfiles[i].OpenReadStream())
                        {
                            using (BinaryReader br = new BinaryReader(st))
                            {
                                byte[] fileBytes = br.ReadBytes((Int32)st.Length);
                                if (fileBytes.Length > 0)
                                {
                                    BPAttachment BPAttachment = new BPAttachment();
                                    BPAttachment.HeaderNumber = TransID;
                                    BPAttachment.AttachmentName = FileName;
                                    BPAttachment.ContentType = ContentType;
                                    BPAttachment.ContentLength = ContentLength;
                                    BPAttachment.AttachmentFile = fileBytes;
                                    //BPAttachment result = await _AttachmentRepository.UpdateAttachment(BPAttachment);
                                    BPAttachment result = await _AttachmentRepository.UploadUserAttachment(BPAttachment, PerviousFileName);

                                    return Ok(result);
                                }
                            }

                        }

                    }
                }

            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Attachment/AddAttachment", ex);
                return BadRequest(ex.Message);
            }
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> UploadUserAttachment()
        {
            try
            {
                var request = Request;
                var TransID = request.Form["TransID"].ToString();
                var CreatedBy = request.Form["CreatedBy"].ToString();
                IFormFileCollection postedfiles = request.Form.Files;

                if (postedfiles.Count > 0)
                {
                    for (int i = 0; i < postedfiles.Count; i++)
                    {
                        var FileName = postedfiles[i].FileName;
                        var ContentType = postedfiles[i].ContentType;
                        var ContentLength = postedfiles[i].Length;
                        using (Stream st = postedfiles[i].OpenReadStream())
                        {
                            using (BinaryReader br = new BinaryReader(st))
                            {
                                byte[] fileBytes = br.ReadBytes((Int32)st.Length);
                                if (fileBytes.Length > 0)
                                {
                                    BPAttachment BPAttachment = new BPAttachment();
                                    BPAttachment.HeaderNumber = TransID;
                                    BPAttachment.AttachmentName = FileName;
                                    BPAttachment.ContentType = ContentType;
                                    BPAttachment.ContentLength = ContentLength;
                                    BPAttachment.AttachmentFile = fileBytes;
                                    BPAttachment result = await _AttachmentRepository.UpdateAttachment(BPAttachment);
                                }
                            }

                        }

                    }
                }

            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Attachment/AddAttachment", ex);
                return BadRequest(ex.Message);
            }
            return Ok();
        }
        [HttpGet]
        public List<BPAttachment> FilterAttachments(string ProjectName, int AppID = 0, string AppNumber = null)
        {
            try
            {
                var attachments = _AttachmentRepository.FilterAttachments(ProjectName, AppID, AppNumber);
                return attachments;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Attachment/FilterAttachments", ex);
                return null;
            }
        }

        [HttpGet]
        public IActionResult DowloandAttachmentByID(int AttachmentID)
        {
            try
            {
                BPAttachment BPAttachment = _AttachmentRepository.GetAttachmentByID(AttachmentID);
                if (BPAttachment != null && BPAttachment.AttachmentFile.Length > 0)
                {
                    Stream stream = new MemoryStream(BPAttachment.AttachmentFile);
                    return File(BPAttachment.AttachmentFile, "application/octet-stream", BPAttachment.AttachmentName);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Attachment/DowloandAttachmentByID", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetIdentityAttachment(string AppNumber, string HeaderNumber, string AttachmentName)
        {
            try
            {
                BPAttachment BPAttachment = _AttachmentRepository.GetIdentityAttachment(AppNumber, HeaderNumber, AttachmentName);
                if (BPAttachment != null && BPAttachment.AttachmentFile.Length > 0)
                {
                    Stream stream = new MemoryStream(BPAttachment.AttachmentFile);
                    return File(BPAttachment.AttachmentFile, "application/octet-stream", BPAttachment.AttachmentName);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Attachment/DowloandAttachmentByID", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<BPAttachment> AddDeclerationAttachment([FromForm] IFormFile[] Files)
        {
            try
            {
                var result = new BPAttachment();
                var files = Files;
                var request = Request;
                var TransID = request.Form["TransID"].ToString();
                var CreatedBy = request.Form["CreatedBy"].ToString();
                var Name = request.Form["Name"].ToString();
                IFormFileCollection postedfiles = request.Form.Files;

                if (postedfiles.Count > 0)
                {
                    for (int i = 0; i < postedfiles.Count; i++)
                    {
                        var FileName = postedfiles[i].FileName;
                        var ContentType = postedfiles[i].ContentType;
                        var ContentLength = postedfiles[i].Length;
                        using (Stream st = postedfiles[i].OpenReadStream())
                        {
                            using (BinaryReader br = new BinaryReader(st))
                            {
                                byte[] fileBytes = br.ReadBytes((Int32)st.Length);
                                if (fileBytes.Length > 0)
                                {
                                    BPAttachment BPAttachment = new BPAttachment();
                                    BPAttachment.HeaderNumber = TransID;
                                    BPAttachment.AttachmentName = FileName;
                                    BPAttachment.ContentType = ContentType;
                                    BPAttachment.ContentLength = ContentLength;
                                    BPAttachment.AttachmentFile = fileBytes;
                                    result = await _AttachmentRepository.AddDeclerationAttachment(BPAttachment, TransID, Name);
                                    if (result != null)
                                    {
                                        try
                                        {
                                            if (Name == "MSME")
                                            {
                                                BPVendorOnBoarding VPVendor = _dbContext.BPVendorOnBoardings.Where(x => x.TransID == int.Parse(TransID)).FirstOrDefault();
                                                VPVendor.MSME_Att_ID = result.AttachmentID.ToString();
                                                await _dbContext.SaveChangesAsync();
                                            }
                                            else if (Name == "RP")
                                            {
                                                BPVendorOnBoarding VPVendor = _dbContext.BPVendorOnBoardings.Where(x => x.TransID == int.Parse(TransID)).FirstOrDefault();
                                                VPVendor.RP_Att_ID = result.AttachmentID.ToString();
                                                await _dbContext.SaveChangesAsync();
                                            }
                                            else if (Name == "LTDS")
                                            {
                                                BPVendorOnBoarding VPVendor = _dbContext.BPVendorOnBoardings.Where(x => x.TransID == int.Parse(TransID)).FirstOrDefault();
                                                VPVendor.TDS_Att_ID = result.AttachmentID.ToString();
                                                await _dbContext.SaveChangesAsync();
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
                                }
                            }

                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public IActionResult DowloandAttachmentByIDAndName(string HeaderNumber, string AttachmentName)
        {
            try
            {
                BPAttachment BPAttachment = _AttachmentRepository.GetAttachmentByIDAndName(HeaderNumber, AttachmentName);
                if (BPAttachment != null && BPAttachment.AttachmentFile.Length > 0)
                {
                    Stream stream = new MemoryStream(BPAttachment.AttachmentFile);
                    return File(BPAttachment.AttachmentFile, "application/octet-stream", BPAttachment.AttachmentName);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Attachment/DowloandAttachmentByID", ex);
                return BadRequest(ex.Message);
            }
        }
    }
}