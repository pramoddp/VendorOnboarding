using BPCloud.VendorRegistrationService.DBContexts;
using BPCloud.VendorRegistrationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorRegistrationService.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly RegistrationContext _dbContext;

        public ContactRepository(RegistrationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<BPContact> GetAllContacts()
        {
            try
            {
                return _dbContext.BPContacts.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPContact> GetContactsByVOB(int TransID)
        {
            try
            {
                return _dbContext.BPContacts.Where(x => x.TransID == TransID).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPContact> CreateContact(BPContact Contact)
        {
            try
            {
                Contact.IsActive = true;
                Contact.CreatedOn = DateTime.Now;
                var result = _dbContext.BPContacts.Add(Contact);
                await _dbContext.SaveChangesAsync();
                return Contact;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateContacts(List<BPContact> Contacts, int TransID)
        {
            try
            {
                if (Contacts != null && Contacts.Count > 0)
                {
                    foreach (BPContact Contact in Contacts)
                    {
                        Contact.TransID = TransID;
                        Contact.Item = Contact.Name;
                        Contact.IsActive = true;
                        Contact.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPContacts.Add(Contact);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPContact> UpdateContact(BPContact Contact)
        {
            try
            {
                var entity = _dbContext.Set<BPContact>().FirstOrDefault(x => x.TransID == Contact.TransID && x.Item == Contact.Item);
                if (entity == null)
                {
                    return entity;
                }
                //_dbContext.Entry(Contact).State = EntityState.Modified;
                entity.Name = Contact.Name;
                entity.Department = Contact.Department;
                entity.Title = Contact.Title;
                entity.Mobile = Contact.Mobile;
                entity.Email = Contact.Email;
                entity.ModifiedBy = Contact.ModifiedBy;
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return Contact;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPContact> DeleteContact(BPContact Contact)
        {
            try
            {
                //var entity = await _dbContext.Set<BPContact>().FindAsync(Contact.Contact, Contact.Language);
                var entity = _dbContext.Set<BPContact>().FirstOrDefault(x => x.TransID == Contact.TransID && x.Item == Contact.Item);
                if (entity == null)
                {
                    return entity;
                }

                _dbContext.Set<BPContact>().Remove(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteContactByTransID(int TransID)
        {
            try
            {
                _dbContext.Set<BPContact>().Where(x => x.TransID == TransID).ToList().ForEach(x => _dbContext.Set<BPContact>().Remove(x));
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
