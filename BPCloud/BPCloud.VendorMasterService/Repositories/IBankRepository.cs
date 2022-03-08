using BPCloud.VendorMasterService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorMasterService.Repositories
{
    public interface IBankRepository
    {
        List<CBPBank> GetAllBanks();
        CBPBank GetBankByIFSC(string IFSC);       
        Task<CBPBank> CreateBank(CBPBank Bank);
        Task<CBPBank> UpdateBank(CBPBank Bank);
        Task<CBPBank> DeleteBank(CBPBank Bank);
    }
}
