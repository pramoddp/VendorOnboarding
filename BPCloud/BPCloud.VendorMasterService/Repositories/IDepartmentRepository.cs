using BPCloud.VendorMasterService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorMasterService.Repositories
{
    public interface IDepartmentRepository
    {
        List<CBPDepartment> GetAllDepartments();
        Task<CBPDepartment> CreateDepartment(CBPDepartment Department);
        Task<CBPDepartment> UpdateDepartment(CBPDepartment Department);
        Task<CBPDepartment> DeleteDepartment(CBPDepartment Department);
    }
}
