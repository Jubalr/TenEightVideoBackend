using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace TenEightVideo.Web.Mail
{
    public class ContactNotificationInfo : IMailData
    {
        public ContactNotificationInfo(string emailAddress)
        {
            EmailAddress = emailAddress;
        }

        public string? Name { get; set; }
        public string? Company { get; set; }
        public string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Subject { get; set; }
        public string? LeadSource { get; set; }
        public string? Message { get; set; }

        public bool TermsAcceptance { get; set; }

        public IXPathNavigable ToIXPathNavigable()
        {
            XmlDocument document = new XmlDocument();
            //Add Main element
            XmlElement info = document.CreateElement("contactNotificationInfo");
            document.AppendChild(info);
            info.AppendTextElement("name", Name!);
            info.AppendTextElement("company", Company!);
            info.AppendTextElement("emailAddress", EmailAddress!);
            info.AppendTextElement("phoneNumber", PhoneNumber!);
            info.AppendTextElement("subject", Subject!);
            info.AppendTextElement("leadSource", LeadSource!);
            info.AppendTextElement("message", Message!);
            info.AppendTextElement("termsAcceptance", TermsAcceptance.ToString());
            return document;
        }
    }
}
