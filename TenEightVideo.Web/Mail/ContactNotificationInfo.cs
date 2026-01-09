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

        public string? InquiryType { get; set; }
        public string? Name { get; set; }
        public string? AgencyOrDepartment { get; set; }
        public string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }        
        public string? Message { get; set; }

        public IXPathNavigable ToIXPathNavigable()
        {
            XmlDocument document = new XmlDocument();
            //Add Main element
            XmlElement info = document.CreateElement("contactNotificationInfo");
            document.AppendChild(info);
            info.AppendTextElement("inquiryType", InquiryType!);
            info.AppendTextElement("name", Name!);
            info.AppendTextElement("agencyOrDepartment", AgencyOrDepartment!);
            info.AppendTextElement("emailAddress", EmailAddress!);
            info.AppendTextElement("phoneNumber", PhoneNumber!);                        
            info.AppendTextElement("message", Message!);            
            return document;
        }
    }
}
