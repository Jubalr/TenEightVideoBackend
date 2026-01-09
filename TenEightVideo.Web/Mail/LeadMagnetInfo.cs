using Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace TenEightVideo.Web.Mail 
{
    public class LeadMagnetInfo : IMailData
    {
        public LeadMagnetInfo(string emailAddress)
        {
            EmailAddress = emailAddress;
        }

        public string? Name { get; set; }
        public string? AgencyOrDepartment { get; set; }
        public string EmailAddress { get; set; }
        public string? PromotionName { get; set; }

        public IXPathNavigable ToIXPathNavigable()
        {
            XmlDocument document = new XmlDocument();
            //Add Main element
            XmlElement info = document.CreateElement("contactNotificationInfo");
            document.AppendChild(info);
            info.AppendTextElement("name", Name ?? string.Empty);
            info.AppendTextElement("agencyOrDepartment", AgencyOrDepartment ?? string.Empty);
            info.AppendTextElement("emailAddress", EmailAddress ?? string.Empty);
            info.AppendTextElement("promotionName", PromotionName ?? string.Empty);
            return document;
        }

    }

}
