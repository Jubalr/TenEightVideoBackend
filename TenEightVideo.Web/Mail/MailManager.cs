using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace TenEightVideo.Web.Mail
{
    public class MailManager : IMailManager
    {
        private static Dictionary<EmailType, XslCompiledTransform> _transforms = null!;        
        private string _contentRootPath;
        private string _mailTransformPath;

        public MailManager(string contentRootPath, string mailTransformPath, ILogger<MailManager> logger)
        {            
            _transforms = new Dictionary<EmailType, XslCompiledTransform>();
            _contentRootPath = contentRootPath;
            _mailTransformPath = mailTransformPath;
            Logger = logger;
        }

        public ILogger<MailManager> Logger { get; }

        public void SendContactNotification(MailAddress sender, MailAddress recipient, ContactNotificationInfo info)
        {
            string subject = "Contact Notification";
            string body = GetEmailBody(info, EmailType.ContactNotification);
            var replyTo = new MailAddress(info.EmailAddress);
            SendEmail(sender, recipient, subject, body, replyTo);
        }

        public void SendEmailSubscriptionNotification(MailAddress sender, MailAddress recipient, SimpleValueInfo info)
        {
            string subject = "Customer Email Subscription Notification";
            string body = GetEmailBody(info, EmailType.EmailSubscriptionNotification);
            SendEmail(sender, recipient, subject, body);
        }

        public void SendTestEmail(MailAddress sender, MailAddress recipient)
        {
            string subject = "10-8Video.com Test E-mail";
            string body = GetTestEmailBody();
            SendEmail(sender, recipient, subject, body);
        }

        public void SendWarrantyNotification(MailAddress sender, MailAddress recipient, WarrantyNotificationInfo info)
        {
            string subject = "Warranty Request Notification";
            string body = GetEmailBody(info, EmailType.WarrantyNotification);
            var replyTo = new MailAddress(info.EmailAddress);

            var headers = new Dictionary<string, string>();
            headers.Add("Form-Warranty-Id", info.RequestId.ToString().Replace("\r\n", string.Empty));
            headers.Add("Form-Company", info.Company?.Replace("\r\n", string.Empty));
            headers.Add("Form-Name", $"{info.FirstName?.Replace("\r\n", string.Empty)} {info.LastName.Replace("\r\n", string.Empty)}");
            headers.Add("Form-Parts-Requested", string.Join("; ", info.PartsRequested.Select(p => $"Part: {p.Name}. Quantity: {p.Quantity}")).Replace("\r\n", string.Empty));
            headers.Add("Form-Problem", info.ProblemDescription?.Replace("\r\n", string.Empty));

            SendEmail(sender, recipient, subject, body, replyTo, headers);
        }

        public void SendWarrantyReport(MailAddress sender, MailAddress recipient, WarrantyReportInfo info, MailAddress bcc)
        {
            string subject = info.Subject;
            string body = GetEmailBody(info, EmailType.WarrantyReport);
            SendEmail(sender, recipient, subject, body, null, null, bcc);
        }

        public void SendLeadMagnetNotification(MailAddress sender, MailAddress recipient, LeadMagnetInfo info)
        {
            string subject = $"Lead Magnet Notification - {info.PromotionName}";
            string body = GetEmailBody(info, EmailType.LeadMagnetNotification);
            var replyTo = new MailAddress(info.EmailAddress);
            SendEmail(sender, recipient, subject, body, replyTo);
        }

        private string GetEmailBody(IMailData data, EmailType type)
        {
            StringBuilder builder = new StringBuilder();
            XslCompiledTransform transform = GetTransform(type);
            IXPathNavigable navigable = data.ToIXPathNavigable();
            using (StringWriter writer = new StringWriter(builder))
            {
                transform.Transform(navigable, null, writer);
                writer.Flush();
                writer.Close();
            }
            return builder.ToString();
        }

        private XslCompiledTransform GetTransform(EmailType type)
        {
            XslCompiledTransform xslt;
            //if (!_transforms.ContainsKey(type))
            //{
            xslt = new XslCompiledTransform();
            string xsltFileName = GetXsltFileName(type);
            xslt.Load(xsltFileName);
            //_transforms.Add(type, xslt);
            Logger.LogInformation($"Loaded mailing stylesheet: {xsltFileName}");
            //}
            //else
            //{
            //    xslt = _transforms[type];
            //}
            return xslt;
        }

        private string GetXsltFileName(EmailType type)
        {
            string fileName = "{0}\\{1}\\{2}.xslt";
            fileName = string.Format(fileName, _contentRootPath, _mailTransformPath, type);
            var uri = new Uri(fileName);
            var converted = uri.AbsoluteUri;
            return converted;
        }

        protected virtual void SendEmail(MailAddress sender, MailAddress recipient, string subject, string body, MailAddress? replyTo = null, Dictionary<string, string>? headers = null, MailAddress? bcc = null)
        {
            //Create client 
            try
            {
                using (SmtpClient client = new SmtpClient())
                {
                    //Send message
                    using (MailMessage msg = new MailMessage())
                    {
                        //Set subject
                        msg.Subject = subject;
                        //Set message format
                        msg.IsBodyHtml = true;
                        //Set From address
                        msg.From = sender;
                        //Set To address
                        msg.To.Add(recipient);
                        //Set body
                        msg.Body = body;
                        //Set reply to address
                        if (replyTo != null)
                            msg.ReplyToList.Add(replyTo);

                        if (bcc != null)
                            msg.Bcc.Add(bcc);

                        if (headers != null && headers.Any())
                        {
                            foreach (var header in headers.Keys)
                            {
                                msg.Headers.Add(header, headers[header]);
                            }
                        }


                        client.Send(msg);
                        Logger.LogInformation($"E-mail sent. Subject: {msg.Subject}; Sender: {sender.Address}; Recipient: {recipient.Address}.");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error sending email");
                throw new Exception("Error sending e-mail:" + ex.Message, ex);
            }
        }

        private string GetTestEmailBody()
        {
            var timeStamp = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
            string emailBody = $@"
            <html>
                <body>
                    <p>
                        <b>Test Message</b>
                    </p>
                    <p>
                        This is a test from the 10-8Video.com website. Please disregard this e-mail.
                    </p>         
                    <p>
                        <b>Time Stamp:</b> {timeStamp}
                    </p>
                    <p>
                        Thanks.
                    </p>
                    <p>
                        The 10-8 Video Team
                    </p>
                </body>
            </html>";
            return emailBody;
        }

        
    }
}
