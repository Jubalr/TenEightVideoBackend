using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace TenEightVideo.Web.Mail
{
    public class SimpleValueInfo : IMailData
    {
        public SimpleValueInfo(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public IXPathNavigable ToIXPathNavigable()
        {
            XmlDocument document = new XmlDocument();
            //Add Main element
            XmlElement info = document.CreateElement("simpleValueInfo");
            document.AppendChild(info);
            info.AppendTextElement("value", Value);

            return document;
        }
    }
}
