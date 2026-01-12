using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenEightVideo.Web.Configuration;

namespace TenEightVideo.JobRunner
{
    public class AppSettings : ApiSettings
    {
        public string? ContentRootPath { get; set; }
        public string? AdministratorEmailAddress { get; internal set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(GMailGoogleUser))
                throw new Exception("GMail Google user must be configured in app settings.");

            if (string.IsNullOrWhiteSpace(GMailGoogleUniqueId))
                throw new Exception("GMail Google unique ID must be configured in app settings.");

            if (string.IsNullOrWhiteSpace(GMailGoogleCertificateFileName))
                throw new Exception("GMail Google certificate file name must be configured in app settings.");

            if (string.IsNullOrWhiteSpace(GMailGoogleCertificatePassword))
                throw new Exception("GMail Google certificate password must be configured in app settings.");

            if (string.IsNullOrWhiteSpace(ContentRootPath))
                throw new Exception("Content root path must be configured in app settings.");

            if (string.IsNullOrWhiteSpace(MailTransformPath))
                throw new Exception("Mail transform path must be configured in app settings.");

            if (string.IsNullOrWhiteSpace(ServerEmailAddress))
                throw new Exception("Server email address must be configured in app settings.");

            if (string.IsNullOrWhiteSpace(ServiceEmailAddress))
                throw new Exception("Service email address must be configured in app settings.");
        }
    }
}   
