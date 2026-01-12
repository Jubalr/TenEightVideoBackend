using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenEightVideo.Web.Data
{
    public class EFWarrantyRequestPartRepository : EFRepository<WarrantyRequestPart>, IWarrantyRequestPartRepository
    {
        public EFWarrantyRequestPartRepository(DbContextOptions<TenEightVideoDbContext> options) 
            : base(options)
        {
        }
    }
}
