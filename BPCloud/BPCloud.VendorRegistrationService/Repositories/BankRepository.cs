using BPCloud.VendorRegistrationService.DBContexts;
using BPCloud.VendorRegistrationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorRegistrationService.Repositories
{
    public class BankRepository : IBankRepository
    {
        private readonly RegistrationContext _dbContext;
        AttachmentRepository attachmentRepository;

        public BankRepository(RegistrationContext dbContext)
        {
            _dbContext = dbContext;
            attachmentRepository = new AttachmentRepository(_dbContext);
        }

        public List<BPBank> GetAllBanks()
        {
            try
            {
                return _dbContext.BPBanks.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPBank> GetBanksByVOB(int TransID)
        {
            try
            {
                return _dbContext.BPBanks.Where(x => x.TransID == TransID).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPBank> CreateBank(BPBank Bank)
        {
            try
            {
                Bank.IsActive = true;
                Bank.CreatedOn = DateTime.Now;
                var result = _dbContext.BPBanks.Add(Bank);
                await _dbContext.SaveChangesAsync();
                return Bank;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateBanks(List<BPBank> Banks, int TransID)
        {
            try
            {
                if (Banks != null && Banks.Count > 0)
                {
                    foreach (BPBank Bank in Banks)
                    {
                        Bank.TransID = TransID;
                        Bank.IsActive = true;
                        Bank.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPBanks.Add(Bank);
                        //if (!string.IsNullOrEmpty(Bank.AttachmentName))
                        //{
                        //    BPAttachment BPAttachment = new BPAttachment();
                        //    BPAttachment.ProjectName = "BPCloud";
                        //    BPAttachment.AppID = 1;
                        //    BPAttachment.AppNumber = result.Entity.AccountNo;
                        //    BPAttachment.IsHeaderExist = true;
                        //    BPAttachment.HeaderNumber = TransID.ToString();
                        //    BPAttachment.AttachmentName = Bank.AttachmentName;
                        //    BPAttachment result1 = await attachmentRepository.AddAttachment(BPAttachment);
                        //    result.Entity.DocID = result1.AttachmentID.ToString();
                        //}
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPBank> UpdateBank(BPBank Bank)
        {
            try
            {
                var entity = _dbContext.Set<BPBank>().FirstOrDefault(x => x.TransID == Bank.TransID && x.AccountNo == Bank.AccountNo);
                if (entity == null)
                {
                    return entity;
                }
                //_dbContext.Entry(Bank).State = EntityState.Modified;
                entity.BankName = Bank.BankName;
                entity.Name = Bank.Name;
                entity.IFSC = Bank.IFSC;
                entity.Branch = Bank.Branch;
                entity.City = Bank.City;
                entity.DocID = Bank.DocID;
                entity.AttachmentName = Bank.AttachmentName;
                entity.IsValid = Bank.IsValid;
                entity.ModifiedBy = Bank.ModifiedBy;
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return Bank;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPBank> DeleteBank(BPBank Bank)
        {
            try
            {
                //var entity = await _dbContext.Set<BPBank>().FindAsync(Bank.Bank, Bank.Language);
                var entity = _dbContext.Set<BPBank>().FirstOrDefault(x => x.TransID == Bank.TransID && x.AccountNo == Bank.AccountNo);
                if (entity == null)
                {
                    return entity;
                }

                _dbContext.Set<BPBank>().Remove(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteBankByTransID(int TransID)
        {
            try
            {
                _dbContext.Set<BPBank>().Where(x => x.TransID == TransID).ToList().ForEach(x => _dbContext.Set<BPBank>().Remove(x));
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
