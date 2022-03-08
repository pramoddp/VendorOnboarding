using BPCloud.VendorMasterService.DBContexts;
using BPCloud.VendorMasterService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorMasterService.Repositories
{
    public class AppRepository : IAppRepository
    {
        private readonly MasterContext _dbContext;

        public AppRepository(MasterContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<CBPApp> GetAllApps()
        {
            try
            {
                return _dbContext.CBPApps.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CBPApp> CreateApp(CBPApp App)
        {
            try
            {
                App.IsActive = true;
                App.CreatedOn = DateTime.Now;
                var result = _dbContext.CBPApps.Add(App);
                await _dbContext.SaveChangesAsync();
                return App;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CBPApp> UpdateApp(CBPApp App)
        {
            try
            {
                var entity = _dbContext.Set<CBPApp>().FirstOrDefault(x => x.ID == App.ID);
                if (entity == null)
                {
                    return entity;
                }
                //_dbContext.Entry(App).State = EntityState.Modified;
                entity.CCode = App.CCode;
                entity.Type = App.Type;
                entity.Level = App.Level;
                entity.User = App.User;
                entity.StartDate = App.StartDate;
                entity.EndDate = App.EndDate;
                entity.ModifiedBy = App.ModifiedBy;
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return App;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CBPApp> DeleteApp(CBPApp App)
        {
            try
            {
                //var entity = await _dbContext.Set<CBPApp>().FindAsync(App.App, App.Language);
                var entity = _dbContext.Set<CBPApp>().FirstOrDefault(x => x.ID == App.ID);
                if (entity == null)
                {
                    return entity;
                }

                _dbContext.Set<CBPApp>().Remove(entity);
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
