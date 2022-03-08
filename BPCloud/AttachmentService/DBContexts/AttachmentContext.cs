using AttachmentService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttachmentService.DBContexts
{
    public class AttachmentContext : DbContext
    {
        public AttachmentContext(DbContextOptions<AttachmentContext> options) : base(options) { }
        public AttachmentContext() { }

        public DbSet<UserAttachment> UserAttachments { get; set; }

    }
}
