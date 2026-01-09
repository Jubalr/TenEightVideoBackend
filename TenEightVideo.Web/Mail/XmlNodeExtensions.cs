using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TenEightVideo.Web.Mail
{
    public static class XmlNodeExtensions
    {
        public static void AppendTextElement(this XmlNode node, string elementName, string elementValue)
        {
            if(node.OwnerDocument == null)
                throw new InvalidOperationException("The XmlNode must be associated with an XmlDocument.");
            
            var element = node.OwnerDocument.CreateElement(elementName);
            element.InnerText = elementValue;
            node.AppendChild(element);
        }
    }
}
