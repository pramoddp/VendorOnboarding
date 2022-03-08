using BPCloud.VendorRegistrationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorRegistrationService.Repositories
{
    public interface IBankRepository
    {
        List<BPBank> GetAllBanks();
        List<BPBank> GetBanksByVOB(int TransID);
        Task<BPBank> CreateBank(BPBank Bank);
        Task CreateBanks(List<BPBank> Banks, int TransID);
        Task<BPBank> UpdateBank(BPBank Bank);
        Task<BPBank> DeleteBank(BPBank Bank);
        Task DeleteBankByTransID(int TransID);
    }
}
