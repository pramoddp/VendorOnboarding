using BPCloud.VendorMasterService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorMasterService.Repositories
{
    public interface IAppRepository
    {
        List<CBPApp> GetAllApps();
        Task<CBPApp> CreateApp(CBPApp App);
        Task<CBPApp> UpdateApp(CBPApp App);
        Task<CBPApp> DeleteApp(CBPApp App);
    }
}
