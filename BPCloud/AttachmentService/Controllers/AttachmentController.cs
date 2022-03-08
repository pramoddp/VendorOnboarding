using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AttachmentService.Models;
using AttachmentService.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AttachmentService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AttachmentController : ControllerBase
    {
        private readonly IAttachmentRepository _AttachmentRepository;
        public AttachmentController(IAttachmentRepository attachmentRepository)
        {
            _AttachmentRepository = attachmentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddAttachment()
        {
            try
            {
                var request = Request;
                var ProjectName = request.Form["ProjectName"].ToString();
                var AppID = int.Parse(request.Form["AppID"].ToString());
                var AppNumber = request.Form["AppNumber"].ToString();
                var IsHeaderExist = bool.Parse(request.Form["IsHeaderExist"].ToString());
                var HeaderNumber = "";
                if (IsHeaderExist)
                {
                    HeaderNumber = request.Form["HeaderNumber"].ToString();
                }
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
                                    UserAttachment userAttachment = new UserAttachment();
                                    userAttachment.ProjectName = ProjectName;
                                    userAttachment.AppID = AppID;
                                    userAttachment.AppNumber = AppNumber;
                                    userAttachment.IsHeaderExist = IsHeaderExist;
                                    if (userAttachment.IsHeaderExist)
                                        userAttachment.HeaderNumber = HeaderNumber;
                                    userAttachment.AttachmentName = FileName;
                                    userAttachment.ContentType = ContentType;
                                    userAttachment.ContentLength = ContentLength;
                                    userAttachment.AttachmentFile = fileBytes;
                                    UserAttachment result = await _AttachmentRepository.AddAttachment(userAttachment);
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
        public List<UserAttachment> FilterAttachments(string ProjectName, int AppID = 0, string AppNumber = null)
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
                UserAttachment userAttachment = _AttachmentRepository.GetAttachmentByID(AttachmentID);
                if (userAttachment != null && userAttachment.AttachmentFile.Length > 0)
                {
                    Stream stream = new MemoryStream(userAttachment.AttachmentFile);
                    return File(userAttachment.AttachmentFile, "application/octet-stream", userAttachment.AttachmentName);
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