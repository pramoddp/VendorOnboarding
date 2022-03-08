using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BPCloud.VendorMasterService.DBContexts;
using BPCloud.VendorMasterService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BPCloud.VendorMasterService.Repositories
{
    public class TypeRepository : ITypeRepository
    {
        private readonly MasterContext _dbContext;

        public TypeRepository(MasterContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<CBPType> GetAllTypes()
        {
            try
            {
                return _dbContext.CBPTypes.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CBPType> CreateType(CBPType type)
        {
            try
            {
                type.IsActive = true;
                type.CreatedOn = DateTime.Now;
                var result = _dbContext.CBPTypes.Add(type);
                await _dbContext.SaveChangesAsync();
                return type;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CBPType> UpdateType(CBPType type)
        {
            try
            {
                var entity = _dbContext.Set<CBPType>().FirstOrDefault(x => x.Type == type.Type && x.Language == type.Language);
                if (entity == null)
                {
                    return entity;
                }
                //_dbContext.Entry(type).State = EntityState.Modified;
                entity.Text = type.Text;
                entity.ModifiedBy = type.ModifiedBy;
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return type;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CBPType> DeleteType(CBPType type)
        {
            try
            {
                //var entity = await _dbContext.Set<CBPType>().FindAsync(type.Type, type.Language);
                var entity = _dbContext.Set<CBPType>().FirstOrDefault(x => x.Type == type.Type && x.Language == type.Language);
                if (entity == null)
                {
                    return entity;
                }

                _dbContext.Set<CBPType>().Remove(entity);
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
