using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TenEightVideo.JobRunner.WarrantyReports
{
    public interface IWarrantyScheduleProcessor
    {
        bool ProcessMonthlyWarrantyReport(MailAddress sender, MailAddress recipient, MailAddress? bcc);
        bool ProcessYearlyWarrantyReport(MailAddress sender, MailAddress recipient, MailAddress? bcc);
    }
}
