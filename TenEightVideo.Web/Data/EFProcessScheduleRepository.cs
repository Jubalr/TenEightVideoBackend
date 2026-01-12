using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenEightVideo.Web.Data
{
    public class EFProcessScheduleRepository : EFRepository<ProcessSchedule>, IProcessScheduleRepository
    {
        public EFProcessScheduleRepository(DbContextOptions<TenEightVideoDbContext> options) : base(options)
        {
        }

        public virtual ProcessSchedule? GetByNameAndSchedule(string processName, int monthly)
        {
            var schedule = GetFirstOrDefault(p => p.ProcessName == processName && p.Schedule == monthly);
            return schedule;
        }
    }
}
