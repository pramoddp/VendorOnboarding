using BPCloud.VendorRegistrationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorRegistrationService.Repositories
{
    public interface IContactRepository
    {
        List<BPContact> GetAllContacts();
        List<BPContact> GetContactsByVOB(int TransID);
        Task<BPContact> CreateContact(BPContact Contact);
        Task CreateContacts(List<BPContact> Contacts, int TransID);
        Task<BPContact> UpdateContact(BPContact Contact);
        Task<BPContact> DeleteContact(BPContact Contact);
        Task DeleteContactByTransID(int TransID);

    }
}
