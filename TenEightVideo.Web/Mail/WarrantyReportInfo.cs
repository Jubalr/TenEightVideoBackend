using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace TenEightVideo.Web.Mail
{
    public class WarrantyReportInfo : IMailData
    {
        public WarrantyReportInfo(string subject)
        {
            Subject = subject;
        }
        public string Subject { get; set; }
        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }
        public IEnumerable<WarrantyPartCountDataRecord>? Records { get; set; }

        public IXPathNavigable ToIXPathNavigable()
        {
            XmlDocument document = new XmlDocument();
            //Add Main element
            XmlElement info = document.CreateElement("warrantyReportInfo");

            info.AppendTextElement("title", Subject);
            info.AppendTextElement("periodStartDate", PeriodStartDate.ToString("MM/dd/yyyy HH:mm:ss"));
            info.AppendTextElement("periodEndDate", PeriodEndDate.ToString("MM/dd/yyyy HH:mm:ss"));
            info.AppendTextElement("totalCount", Records?.Sum(r => r.Count).ToString() ?? "0");

            var records = document.CreateElement("records");
            foreach (var record in Records ?? [])
            {
                var xmlRecord = document.CreateElement("record");

                xmlRecord.AppendTextElement("partRequested", record.PartRequested!);
                xmlRecord.AppendTextElement("count", record.Count.ToString());

                records.AppendChild(xmlRecord);
            }
            info.AppendChild(records);
            document.AppendChild(info);
            return document;
        }
    }
}
