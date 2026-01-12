using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenEightVideo.Web.Data
{
    public class PagingInfo
    {
        public PagingInfo(int pageNumber, int pageSize, int totalRecordCount)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecordCount = totalRecordCount;
        }

        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalRecordCount { get; }

        public int PageCount
        {
            get
            {
                var pageCount = TotalRecordCount / PageSize;
                if ((TotalRecordCount % PageSize) > 0)
                    pageCount++;
                return pageCount;
            }
        }
    }
}
