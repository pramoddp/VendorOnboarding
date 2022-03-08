using BPCloud.VendorMasterService.DBContexts;
using BPCloud.VendorMasterService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorMasterService.Repositories
{
    public class PostalRepository : IPostalRepository
    {
        private readonly MasterContext _dbContext;

        public PostalRepository(MasterContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<CBPPostal> GetAllPostals()
        {
            try
            {
                return _dbContext.CBPPostals.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CBPPostal> CreatePostal(CBPPostal Postal)
        {
            try
            {
                Postal.IsActive = true;
                Postal.CreatedOn = DateTime.Now;
                var result = _dbContext.CBPPostals.Add(Postal);
                await _dbContext.SaveChangesAsync();
                return Postal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CBPPostal> UpdatePostal(CBPPostal Postal)
        {
            try
            {
                var entity = _dbContext.Set<CBPPostal>().FirstOrDefault(x => x.PostalCode == Postal.PostalCode);
                if (entity == null)
                {
                    return entity;
                }
                //_dbContext.Entry(Postal).State = EntityState.Modified;
                entity.Country = Postal.Country;
                entity.State = Postal.State;
                entity.City = Postal.City;
                entity.AddressLine1 = Postal.AddressLine1;
                entity.AddressLine2 = Postal.AddressLine2;
                entity.ModifiedBy = Postal.ModifiedBy;
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return Postal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CBPPostal> DeletePostal(CBPPostal Postal)
        {
            try
            {
                //var entity = await _dbContext.Set<CBPPostal>().FindAsync(Postal.Postal, Postal.Language);
                var entity = _dbContext.Set<CBPPostal>().FirstOrDefault(x => x.PostalCode == Postal.PostalCode);
                if (entity == null)
                {
                    return entity;
                }

                _dbContext.Set<CBPPostal>().Remove(entity);
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
