using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace TenEightVideo.Web.Mail
{
    public class WarrantyNotificationInfo : IMailData
    {
        public WarrantyNotificationInfo(string emailAddress)
        {
            EmailAddress = emailAddress;
        }
        public long RequestId { get; set; }

        public string? Company { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string EmailAddress { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? ZipCode { get; set; }

        public string? Country { get; set; }

        public IEnumerable<WarrantyPart>? PartsRequested { get; set; }

        public string? ProblemDescription { get; set; }

        public bool TermsAcceptance { get; set; }

        public IXPathNavigable ToIXPathNavigable()
        {
            XmlDocument document = new XmlDocument();
            //Add Main element
            XmlElement info = document.CreateElement("warrantyNotificationInfo");
            document.AppendChild(info);
            info.AppendTextElement("requestId", RequestId.ToString());
            info.AppendTextElement("company", Company!);
            info.AppendTextElement("firstName", FirstName!);
            info.AppendTextElement("lastName", LastName!);
            info.AppendTextElement("emailAddress", EmailAddress!);
            info.AppendTextElement("phoneNumber", PhoneNumber!);
            info.AppendTextElement("address1", Address1!);
            info.AppendTextElement("address2", Address2!);
            info.AppendTextElement("city", City!);
            info.AppendTextElement("state", State!);
            info.AppendTextElement("zipCode", ZipCode!);
            info.AppendTextElement("country", Country!);
            info.AppendTextElement("problemDescription", ProblemDescription!);
            info.AppendTextElement("termsAcceptance", TermsAcceptance.ToString());

            var parts = document.CreateElement("warrantyParts");
            foreach (var part in PartsRequested ?? [])
            {
                var partElement = document.CreateElement("part");
                partElement.AppendTextElement("name", part.Name!);
                partElement.AppendTextElement("qty", part.Quantity.ToString());
                parts.AppendChild(partElement);
            }
            info.AppendChild(parts);

            return document;
        }
    }
}
