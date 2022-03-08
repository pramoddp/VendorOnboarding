using BPCloud.VendorMasterService.DBContexts;
using BPCloud.VendorMasterService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorMasterService.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly MasterContext _dbContext;

        public IdentityRepository(MasterContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<CBPIdentity> GetAllIdentities()
        {
            try
            {
                return _dbContext.CBPIdentities.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> GetAllIdentityTypes()
        {
            try
            {
                return _dbContext.CBPIdentities.Select(x => x.Text).Distinct().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CBPIdentity GetIdentityByType(string Type)
        {
            try
            {
                return _dbContext.CBPIdentities.FirstOrDefault(x => x.Text.ToLower() == Type.ToLower());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CBPIdentity ValidateIdentityByType(string Type)
        {
            try
            {
                return _dbContext.CBPIdentities.FirstOrDefault(x => x.Text.ToLower() == Type.ToLower());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CBPIdentity> CreateIdentity(CBPIdentity Identity)
        {
            try
            {
                Identity.IsActive = true;
                Identity.CreatedOn = DateTime.Now;
                var result = _dbContext.CBPIdentities.Add(Identity);
                await _dbContext.SaveChangesAsync();
                return Identity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CBPIdentity> UpdateIdentity(CBPIdentity Identity)
        {
            try
            {
                var entity = _dbContext.Set<CBPIdentity>().FirstOrDefault(x => x.ID == Identity.ID);
                if (entity == null)
                {
                    return entity;
                }
                //_dbContext.Entry(Identity).State = EntityState.Modified;
                entity.Type = Identity.Type;
                entity.Text = Identity.Text;
                entity.RegexFormat = Identity.RegexFormat;
                entity.Mandatory = Identity.Mandatory;
                entity.FileFormat = Identity.FileFormat;
                entity.MaxSizeInKB = Identity.MaxSizeInKB;
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

        public async Task<CBPIdentity> DeleteIdentity(CBPIdentity Identity)
        {
            try
            {
                //var entity = await _dbContext.Set<CBPIdentity>().FindAsync(Identity.Identity, Identity.Language);
                var entity = _dbContext.Set<CBPIdentity>().FirstOrDefault(x => x.ID == Identity.ID);
                if (entity == null)
                {
                    return entity;
                }

                _dbContext.Set<CBPIdentity>().Remove(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
