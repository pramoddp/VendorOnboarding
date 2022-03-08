using BPCloud.VendorMasterService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorMasterService.Repositories
{
    public interface IPostalRepository
    {
        List<CBPPostal> GetAllPostals();
        Task<CBPPostal> CreatePostal(CBPPostal Postal);
        Task<CBPPostal> UpdatePostal(CBPPostal Postal);
        Task<CBPPostal> DeletePostal(CBPPostal Postal);
    }
}
