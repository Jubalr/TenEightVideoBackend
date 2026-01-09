using System.ComponentModel.DataAnnotations;
using TenEightVideo.Web.Mail;

namespace TenEightVideo.Web.Services.Models
{
    public class WarrantyModel
    {        
        public string? Company { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string EmailAddress { get; set; }

        public string? PhoneNumber { get; set; }

        public string SerialNumber { get; set; }
        
        public IEnumerable<WarrantyPart>? PartsRequested { get; set; }
        
        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? ZipCode { get; set; }

        public string? Country { get; set; }
       
        public string? ProblemDescription { get; set; }

        public bool TermsAcceptance { get; set; }
    }
}
