using BPCloud.VendorRegistrationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorRegistrationService.Repositories
{
    public interface IIdentityRepository
    {
        List<BPIdentity> GetAllIdentities();
        List<BPIdentity> GetIdentitiesByVOB(int TransID);
        Task<BPIdentity> CreateIdentity(BPIdentity Identity);
        Task CreateIdentities(List<BPIdentity> Identities, int TransID);
        Task<BPIdentity> UpdateIdentity(BPIdentity Identity);
        Task<BPIdentity> DeleteIdentity(BPIdentity Identity);
        Task DeleteIdentityByTransID(int TransID);
    }
}
