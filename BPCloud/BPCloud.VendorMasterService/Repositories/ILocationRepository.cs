using BPCloud.VendorMasterService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorMasterService.Repositories
{
    public interface ILocationRepository
    {
        CBPLocation GetLocationByPincode(string Pincode);
        List<StateDetails> GetStateDetails();
        List<MyArray> GetLocation(string Pincode);
    }
}
