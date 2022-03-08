using BPCloud.VendorMasterService.DBContexts;
using BPCloud.VendorMasterService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorMasterService.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly MasterContext _dbContext;

        public DepartmentRepository(MasterContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<CBPDepartment> GetAllDepartments()
        {
            try
            {
                return _dbContext.CBPDepartments.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CBPDepartment> CreateDepartment(CBPDepartment Department)
        {
            try
            {
                Department.IsActive = true;
                Department.CreatedOn = DateTime.Now;
                var result = _dbContext.CBPDepartments.Add(Department);
                await _dbContext.SaveChangesAsync();
                return Department;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CBPDepartment> UpdateDepartment(CBPDepartment Department)
        {
            try
            {
                var entity = _dbContext.Set<CBPDepartment>().FirstOrDefault(x => x.Department == Department.Department && x.Language == Department.Language);
                if (entity == null)
                {
                    return entity;
                }
                //_dbContext.Entry(Department).State = EntityState.Modified;
                entity.Text = Department.Text;
                entity.ModifiedBy = Department.ModifiedBy;
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return Department;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CBPDepartment> DeleteDepartment(CBPDepartment Department)
        {
            try
            {
                //var entity = await _dbContext.Set<CBPDepartment>().FindAsync(Department.Department, Department.Language);
                var entity = _dbContext.Set<CBPDepartment>().FirstOrDefault(x => x.Department == Department.Department && x.Language == Department.Language);
                if (entity == null)
                {
                    return entity;
                }

                _dbContext.Set<CBPDepartment>().Remove(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
