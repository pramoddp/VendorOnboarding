using BPCloud.VendorMasterService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorMasterService.Repositories
{
    public interface IFieldMasterRepository
    {
        List<CBPFieldMaster> GetAllOnBoardingFieldMaster();
        List<CBPIdentity> GetAllIdentityFields();
        Task<CBPFieldMaster> UpdateOnBoardingFieldMaster(CBPFieldMaster OnBoardingFieldMaster);
    }
}
