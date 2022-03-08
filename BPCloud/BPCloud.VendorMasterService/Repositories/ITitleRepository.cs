using BPCloud.VendorMasterService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorMasterService.Repositories
{
    public interface ITitleRepository
    {
        List<CBPTitle> GetAllTitles();
        Task<CBPTitle> CreateTitle(CBPTitle Title);
        Task<CBPTitle> UpdateTitle(CBPTitle Title);
        Task<CBPTitle> DeleteTitle(CBPTitle Title);
    }
}
