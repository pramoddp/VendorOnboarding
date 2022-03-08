using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorRegistrationService.Repositories
{
    public interface IServiceRepository
    {
        Task SendReminderToInitializedVendor();
        Task SendReminderToSavedVendor();
        Task SendReminderToRegisteredVendor();
    }
}
