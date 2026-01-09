using System.ComponentModel.DataAnnotations.Schema;

namespace TenEightVideo.Web.Data
{
    [Table("WarrantyRequest")]
    public class WarrantyRequest
    {
        public long Id { get; set; }
        public string? Company { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
        public string? ProblemDescription { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public string? UpdatedBy { get; set; }        
        public ICollection<WarrantyRequestPart> WarrantyRequestParts { get; set; } = new List<WarrantyRequestPart>();
    }
}