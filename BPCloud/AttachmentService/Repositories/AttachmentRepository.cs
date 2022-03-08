using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttachmentService.DBContexts;
using AttachmentService.Models;

namespace AttachmentService.Repositories
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly AttachmentContext _dbContext;

        public AttachmentRepository(AttachmentContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<UserAttachment> AddAttachment(UserAttachment userAttachment)
        {
            try
            {
                userAttachment.IsActive = true;
                userAttachment.CreatedOn = DateTime.Now;
                var result = _dbContext.UserAttachments.Add(userAttachment);
                await _dbContext.SaveChangesAsync();
                return userAttachment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<UserAttachment> FilterAttachments(string ProjectName, int AppID = 0, string AppNumber = null)
        {
            try
            {
                bool IsAppID = AppID > 0;
                bool IsAppNumber = !string.IsNullOrEmpty(AppNumber);
                var result = (from tb in _dbContext.UserAttachments
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

        public UserAttachment GetAttachmentByID(int AttachmentID)
        {
            try
            {
                var userAttachment = _dbContext.UserAttachments.FirstOrDefault(x => x.AttachmentID == AttachmentID);
                return userAttachment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
