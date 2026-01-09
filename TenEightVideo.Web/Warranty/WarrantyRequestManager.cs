using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TenEightVideo.Web.Data;

namespace TenEightVideo.Web.Warranty
{
    public class WarrantyRequestManager : IWarrantyRequestManager
    {
        private readonly TenEightVideoDbContext _context;

        public WarrantyRequestManager(TenEightVideoDbContext context)
        {
            _context = context;
        }

        public void CreateRequest(WarrantyRequest request)
        {
            request.DateCreated = DateTime.Now;
            request.CreatedBy = Thread.CurrentPrincipal?.Identity?.Name;

            foreach (var part in request.WarrantyRequestParts ?? [])
            {
                part.DateCreated = DateTime.Now;
                part.CreatedBy = Thread.CurrentPrincipal?.Identity?.Name;
            }
            _context.WarrantyRequests.Add(request);
            _context.SaveChanges();
        }
    }
}
