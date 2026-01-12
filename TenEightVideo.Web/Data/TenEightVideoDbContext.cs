using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenEightVideo.Web.Data
{
    public partial class TenEightVideoDbContext : DbContext
    {
        public TenEightVideoDbContext(DbContextOptions<TenEightVideoDbContext> options)
            : base(options)
        {
        }
             
        public DbSet<WarrantyRequestPart> WarrantyRequestParts { get; set; }
        public DbSet<WarrantyRequest> WarrantyRequests { get; set; }

        public DbSet<ProcessSchedule> ProcessSchedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WarrantyRequestPart>()
                .HasOne(wrp => wrp.WarrantyRequest)
                .WithMany(wr => wr.WarrantyRequestParts)
                .HasForeignKey(wrp => wrp.WarrantyRequestId);
        }
    }
}
