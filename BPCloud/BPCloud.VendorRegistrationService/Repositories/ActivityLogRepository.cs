using BPCloud.VendorRegistrationService.DBContexts;
using BPCloud.VendorRegistrationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorRegistrationService.Repositories
{
    public class ActivityLogRepository : IActivityLogRepository
    {
        private readonly RegistrationContext _dbContext;

        public ActivityLogRepository(RegistrationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<BPActivityLog> GetAllActivityLogs()
        {
            try
            {
                return _dbContext.BPActivityLogs.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPActivityLog> GetActivityLogsByVOB(int TransID)
        {
            try
            {
                return _dbContext.BPActivityLogs.Where(x => x.TransID == TransID).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPActivityLog> CreateActivityLog(BPActivityLog ActivityLog)
        {
            try
            {
                ActivityLog.IsActive = true;
                ActivityLog.CreatedOn = DateTime.Now;
                var result = _dbContext.BPActivityLogs.Add(ActivityLog);
                await _dbContext.SaveChangesAsync();
                return ActivityLog;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateActivityLogs(List<BPActivityLog> ActivityLogs, int TransID)
        {
            try
            {
                if (ActivityLogs != null && ActivityLogs.Count > 0)
                {
                    foreach (BPActivityLog ActivityLog in ActivityLogs)
                    {
                        ActivityLog.TransID = TransID;
                        ActivityLog.IsActive = true;
                        ActivityLog.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPActivityLogs.Add(ActivityLog);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPActivityLog> UpdateActivityLog(BPActivityLog ActivityLog)
        {
            try
            {
                var entity = _dbContext.Set<BPActivityLog>().FirstOrDefault(x => x.TransID == ActivityLog.TransID && x.LogID == ActivityLog.LogID);
                if (entity == null)
                {
                    return entity;
                }
                //_dbContext.Entry(ActivityLog).State = EntityState.Modified;
                entity.Activity = ActivityLog.Activity;
                entity.Text = ActivityLog.Text;
                entity.Date = ActivityLog.Date;
                entity.Time = ActivityLog.Time;
                entity.ModifiedBy = ActivityLog.ModifiedBy;
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return ActivityLog;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPActivityLog> DeleteActivityLog(BPActivityLog ActivityLog)
        {
            try
            {
                //var entity = await _dbContext.Set<BPActivityLog>().FindAsync(ActivityLog.ActivityLog, ActivityLog.Language);
                var entity = _dbContext.Set<BPActivityLog>().FirstOrDefault(x => x.TransID == ActivityLog.TransID && x.LogID == ActivityLog.LogID);
                if (entity == null)
                {
                    return entity;
                }

                _dbContext.Set<BPActivityLog>().Remove(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteActivityLogByTransID(int TransID)
        {
            try
            {
                _dbContext.Set<BPActivityLog>().Where(x => x.TransID == TransID).ToList().ForEach(x => _dbContext.Set<BPActivityLog>().Remove(x));
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
