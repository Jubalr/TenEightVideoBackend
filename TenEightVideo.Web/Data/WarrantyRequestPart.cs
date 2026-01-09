using System.ComponentModel.DataAnnotations.Schema;

namespace TenEightVideo.Web.Data
{
    [Table("WarrantyRequestPart")]
    public class WarrantyRequestPart
    {
        public long Id { get; set; }
        public long WarrantyRequestId { get; set; }
        public string? PartRequested { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public string? UpdatedBy { get; set; }
        public int Quantity { get; set; }

        public WarrantyRequest WarrantyRequest { get; set; }
    }
}