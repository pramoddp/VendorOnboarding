using BPCloud.VendorMasterService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorMasterService.Repositories
{
    public interface ITypeRepository
    {
        List<CBPType> GetAllTypes();
        Task<CBPType> CreateType(CBPType type);
        Task<CBPType> UpdateType(CBPType type);
        Task<CBPType> DeleteType(CBPType type);
    }
}
