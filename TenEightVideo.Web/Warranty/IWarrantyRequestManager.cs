using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenEightVideo.Web.Data;

namespace TenEightVideo.Web.Warranty
{
    public interface IWarrantyRequestManager
    {
        void CreateRequest(WarrantyRequest request);
    }
}
