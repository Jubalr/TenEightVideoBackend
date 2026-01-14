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
        public int EstimatedFleetSize { get; set; }
        public string JobTitle { get; set; }
        public string PdfSpecsRequested { get; set; }
        public string PreferredContactMethod { get; set; }
        public string State { get; set; }
        public string SystemsOfInterest { get; set; }

        public IXPathNavigable ToIXPathNavigable()
        {
            XmlDocument document = new XmlDocument();
            //Add Main element
            XmlElement info = document.CreateElement("contactNotificationInfo");
            document.AppendChild(info);
            info.AppendTextElement("inquiryType", InquiryType ?? string.Empty);
            info.AppendTextElement("name", Name ?? string.Empty);
            info.AppendTextElement("agencyOrDepartment", AgencyOrDepartment ?? string.Empty);
            info.AppendTextElement("emailAddress", EmailAddress ?? string.Empty);
            info.AppendTextElement("phoneNumber", PhoneNumber ?? string.Empty);                        
            info.AppendTextElement("message", Message ?? string.Empty);
            info.AppendTextElement("estimatedFleetSize", EstimatedFleetSize > 0 ? EstimatedFleetSize.ToString() : string.Empty);
            info.AppendTextElement("jobTitle", JobTitle ?? string.Empty);
            info.AppendTextElement("pdfSpecsRequested", PdfSpecsRequested ?? string.Empty);
            info.AppendTextElement("preferredContactMethod", PreferredContactMethod ?? string.Empty);
            info.AppendTextElement("state", State ?? string.Empty);
            info.AppendTextElement("systemsOfInterest", SystemsOfInterest ?? string.Empty);
            return document;
        }
    }
}
