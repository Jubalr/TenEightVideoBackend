using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenEightVideo.Web.Configuration
{
    public class ApiSettings
    {
        public const string SECTION_NAME = "ApiSettings";
        public string? MailTransformPath { get; set; } 
        public string? GMailGoogleUniqueId { get; set; }
        public string? GMailGoogleUser { get; set; }
        public string? GMailGoogleCertificateFileName { get; set; }
        public string? GMailGoogleCertificatePassword { get; set; }
        public string? ServerEmailAddress { get; set; }
        public string? ServiceEmailAddress { get; set; }
    }
}
