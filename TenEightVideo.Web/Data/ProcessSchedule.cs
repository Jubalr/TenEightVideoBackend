using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenEightVideo.Web.Data
{
    [Table("ProcessSchedule")]
    public partial class ProcessSchedule
    {
        public long Id { get; set; }
        public string? ProcessName { get; set; }
        public int Schedule { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<System.DateTime> DateLastProcessed { get; set; }
    }
}
