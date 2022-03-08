using BPCloud.VendorRegistrationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorRegistrationService.Repositories
{
    public interface IAttachmentRepository
    {
        Task DeleteAttachment(string AppNumber, string HeaderNumber);
        Task<BPAttachment> AddAttachment(BPAttachment BPAttachment);
        Task<BPAttachment> UpdateAttachment(BPAttachment BPAttachment);
        Task<BPAttachment> UploadUserAttachment(BPAttachment BPAttachment,string perviousName);
        List<BPAttachment> FilterAttachments(string ProjectName, int AppID = 0, string AppNumber = null);
        BPAttachment FilterAttachment(string ProjectName, string AttchmentName, int AppID = 0, string AppNumber = null, string HeaderNumber = null);
        BPAttachment GetAttachmentByName(string AttachmentName);
        BPAttachment GetAttachmentByID(int AttachmentID);

        Task<BPAttachment> AddDeclerationAttachment(BPAttachment attachmentData, string TransID, string Name);
        BPAttachment GetIdentityAttachment(string AppNumber, string HeaderNumber, string AttachmentName);

        void UpdateDecelrationId(BPAttachment attachmentData, string TransID, string Name);
        BPAttachment GetAttachmentByIDAndName(string HeaderNumber, string AttachmentName);
    }
}
