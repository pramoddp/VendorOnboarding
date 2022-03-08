using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BPCloud.VendorMasterService.DBContexts;
using BPCloud.VendorMasterService.Models;

namespace BPCloud.VendorMasterService.Repositories
{
    public class FieldMasterRepository : IFieldMasterRepository
    {
        private readonly MasterContext _dbContext;

        public FieldMasterRepository(MasterContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<CBPIdentity> GetAllIdentityFields()
        {
            try
            {
                var result = _dbContext.CBPIdentities.ToList();
                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<CBPFieldMaster> GetAllOnBoardingFieldMaster()
        {
            try
            {
                return _dbContext.CBPFieldMasters.ToList();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("FieldMasterRepository/GetAllOnBoardingFieldMaster : - ", ex);
                throw ex;
            }
        }

        public async Task<CBPFieldMaster> UpdateOnBoardingFieldMaster(CBPFieldMaster OnBoardingFieldMaster)
        {
            try
            {
                CBPFieldMaster OnBoardingFieldMaster2 = (from tb in _dbContext.CBPFieldMasters
                                                         where tb.IsActive && tb.ID == OnBoardingFieldMaster.ID
                                                         select tb).FirstOrDefault();
                OnBoardingFieldMaster2.Text = OnBoardingFieldMaster.Text;
                OnBoardingFieldMaster2.DefaultValue = OnBoardingFieldMaster.DefaultValue;
                OnBoardingFieldMaster2.Mandatory = OnBoardingFieldMaster.Mandatory;
                OnBoardingFieldMaster2.Invisible = OnBoardingFieldMaster.Invisible;
                OnBoardingFieldMaster2.IsActive = true;
                OnBoardingFieldMaster2.ModifiedOn = DateTime.Now;
                OnBoardingFieldMaster2.ModifiedBy = OnBoardingFieldMaster.ModifiedBy;
                await _dbContext.SaveChangesAsync();
                return OnBoardingFieldMaster2;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("FieldMasterRepository/UpdateOnBoardingFieldMaster : - ", ex);
                throw ex;
            }
        }
    }
}
