using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenEightVideo.Web.Data
{
    public interface IProcessScheduleRepository : IRepository<ProcessSchedule>
    {
        ProcessSchedule? GetByNameAndSchedule(string processName, int monthly);
    }
}
