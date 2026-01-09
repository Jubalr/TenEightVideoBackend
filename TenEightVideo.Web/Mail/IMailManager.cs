using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TenEightVideo.Web.Mail
{
    public interface IMailManager
    {
        void SendEmailSubscriptionNotification(MailAddress sender, MailAddress recipient, SimpleValueInfo info);
        void SendContactNotification(MailAddress sender, MailAddress recipient, ContactNotificationInfo info);
        void SendTestEmail(MailAddress sender, MailAddress recipient);
        void SendWarrantyNotification(MailAddress sender, MailAddress recipient, WarrantyNotificationInfo info);        
        void SendWarrantyReport(MailAddress sender, MailAddress recipient, WarrantyReportInfo info, MailAddress bcc);
        void SendLeadMagnetNotification(MailAddress sender, MailAddress recipient, LeadMagnetInfo info);
    }
}
