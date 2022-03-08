using BPCloud.VendorRegistrationService.DBContexts;
using BPCloud.VendorRegistrationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorRegistrationService.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly RegistrationContext _dbContext;
        AttachmentRepository attachmentRepository;

        public IdentityRepository(RegistrationContext dbContext)
        {
            _dbContext = dbContext;
            attachmentRepository = new AttachmentRepository(_dbContext);
        }

        public List<BPIdentity> GetAllIdentities()
        {
            try
            {
                return _dbContext.BPIdentities.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPIdentity> GetIdentitiesByVOB(int TransID)
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

        public async Task<BPIdentity> CreateIdentity(BPIdentity Identity)
        {
            try
            {
                Identity.IsActive = true;
                Identity.CreatedOn = DateTime.Now;
                var result = _dbContext.BPIdentities.Add(Identity);
                await _dbContext.SaveChangesAsync();
                return Identity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateIdentities(List<BPIdentity> Identities, int TransID)
        {
            try
            {
                if (Identities != null && Identities.Count > 0)
                {
                    foreach (BPIdentity Identity in Identities)
                    {
                        Identity.TransID = TransID;
                        Identity.IsActive = true;
                        Identity.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPIdentities.Add(Identity);
                        if (!string.IsNullOrEmpty(Identity.AttachmentName))
                        {
                            //BPAttachment BPAttachment = new BPAttachment();
                            //BPAttachment.ProjectName = "BPCloud";
                            //BPAttachment.AppID = 1;
                            //BPAttachment.AppNumber = result.Entity.Type;
                            //BPAttachment.IsHeaderExist = true;
                            //BPAttachment.HeaderNumber = TransID.ToString();
                            //BPAttachment.AttachmentName = Identity.AttachmentName;
                            //BPAttachment result1 = await attachmentRepository.AddAttachment(BPAttachment);
                            //result.Entity.DocID = result1.AttachmentID.ToString();

                            BPAttachment attachment = _dbContext.BPAttachments.Where(x => x.HeaderNumber == TransID.ToString() && x.AttachmentName == Identity.AttachmentName).FirstOrDefault();
                            if(attachment != null)
                            {
                                attachment.ProjectName = "BPCloud";
                                attachment.AppID = 1;
                                attachment.IsHeaderExist = true;
                                attachment.AppNumber = result.Entity.Type;
                                attachment.ProjectName = "BPCloud";
                                result.Entity.DocID = attachment.AttachmentID.ToString();
                            }
                        }
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPIdentity> UpdateIdentity(BPIdentity Identity)
        {
            try
            {
                var entity = _dbContext.Set<BPIdentity>().FirstOrDefault(x => x.TransID == Identity.TransID && x.Type == Identity.Type);
                if (entity == null)
                {
                    return entity;
                }
                //_dbContext.Entry(Identity).State = EntityState.Modified;
                entity.IDNumber = Identity.IDNumber;
                entity.DocID = Identity.DocID;
                entity.AttachmentName = Identity.AttachmentName;
                entity.ValidUntil = Identity.ValidUntil;
                entity.IsValid = Identity.IsValid;
                entity.ModifiedBy = Identity.ModifiedBy;
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return Identity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPIdentity> DeleteIdentity(BPIdentity Identity)
        {
            try
            {
                //var entity = await _dbContext.Set<BPIdentity>().FindAsync(Identity.Identity, Identity.Language);
                var entity = _dbContext.Set<BPIdentity>().FirstOrDefault(x => x.TransID == Identity.TransID && x.Type == Identity.Type);
                if (entity == null)
                {
                    return entity;
                }

                _dbContext.Set<BPIdentity>().Remove(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteIdentityByTransID(int TransID)
        {
            try
            {
                _dbContext.Set<BPIdentity>().Where(x => x.TransID == TransID).ToList().ForEach(x => _dbContext.Set<BPIdentity>().Remove(x));
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
