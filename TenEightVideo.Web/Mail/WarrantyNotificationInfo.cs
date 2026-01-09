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
            info.AppendTextElement("company", Company ?? string.Empty);
            info.AppendTextElement("firstName", FirstName ?? string.Empty);
            info.AppendTextElement("lastName", LastName ?? string.Empty);
            info.AppendTextElement("emailAddress", EmailAddress ?? string.Empty);
            info.AppendTextElement("phoneNumber", PhoneNumber ?? string.Empty);
            info.AppendTextElement("address1", Address1 ?? string.Empty);
            info.AppendTextElement("address2", Address2 ?? string.Empty);
            info.AppendTextElement("city", City ?? string.Empty);
            info.AppendTextElement("state", State ?? string.Empty);
            info.AppendTextElement("zipCode", ZipCode ?? string.Empty);
            info.AppendTextElement("country", Country ?? string.Empty);
            info.AppendTextElement("problemDescription", ProblemDescription ?? string.Empty);
            info.AppendTextElement("termsAcceptance", TermsAcceptance.ToString());

            var parts = document.CreateElement("warrantyParts");
            foreach (var part in PartsRequested ?? [])
            {
                var partElement = document.CreateElement("part");
                partElement.AppendTextElement("name", part.Name ?? string.Empty);
                partElement.AppendTextElement("qty", part.Quantity.ToString());
                parts.AppendChild(partElement);
            }
            info.AppendChild(parts);

            return document;
        }
    }
}
