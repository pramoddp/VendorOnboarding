using BPCloud.VendorMasterService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorMasterService.Repositories
{
    public interface IIdentityRepository
    {
        List<CBPIdentity> GetAllIdentities();
        List<string> GetAllIdentityTypes();       
        CBPIdentity GetIdentityByType(string Type);
        CBPIdentity ValidateIdentityByType(string Type);
        Task<CBPIdentity> CreateIdentity(CBPIdentity Identity);
        Task<CBPIdentity> UpdateIdentity(CBPIdentity Identity);
        Task<CBPIdentity> DeleteIdentity(CBPIdentity Identity);
    }
}
