using BPCloud.VendorRegistrationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorRegistrationService.Repositories
{
    public interface IActivityLogRepository
    {
        List<BPActivityLog> GetAllActivityLogs();
        List<BPActivityLog> GetActivityLogsByVOB(int TransID);
        Task<BPActivityLog> CreateActivityLog(BPActivityLog ActivityLog);
        Task CreateActivityLogs(List<BPActivityLog> ActivityLogs, int TransID);
        Task<BPActivityLog> UpdateActivityLog(BPActivityLog ActivityLog);
        Task<BPActivityLog> DeleteActivityLog(BPActivityLog ActivityLog);
        Task DeleteActivityLogByTransID(int TransID);

    }
}
