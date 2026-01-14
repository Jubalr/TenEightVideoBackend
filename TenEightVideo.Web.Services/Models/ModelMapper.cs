using TenEightVideo.Web.Data;
using TenEightVideo.Web.Mail;

namespace TenEightVideo.Web.Services.Models
{
    public class ModelMapper
    {
        public static void Map(WarrantyModel model, WarrantyRequest request)
        {
            request.Company = model.Company;
            request.FirstName = model.FirstName;
            request.LastName = model.LastName;
            request.EmailAddress = model.EmailAddress;
            request.PhoneNumber = model.PhoneNumber;
            request.SerialNumber = model.SerialNumber;
            request.Address1 = model.Address1;
            request.Address2 = model.Address2;
            request.City = model.City;
            request.State = model.State;
            request.ZipCode = model.ZipCode;
            request.Country = model.Country;
            request.ProblemDescription = model.ProblemDescription;

            foreach (var part in model.PartsRequested ?? [])
            {
                var warrantyRequestPart = new WarrantyRequestPart()
                {
                    PartRequested = part.Name,
                    Quantity = part.Quantity
                };
                request.WarrantyRequestParts!.Add(warrantyRequestPart);
            }
        }

        public static WarrantyNotificationInfo Map(WarrantyModel model)
        {
            var info = new WarrantyNotificationInfo();
            info.Company = model.Company;
            info.FirstName = model.FirstName;
            info.LastName = model.LastName;
            info.EmailAddress = model.EmailAddress;
            info.PhoneNumber = model.PhoneNumber;
            info.SerialNumber = model.SerialNumber;
            info.Address1 = model.Address1;
            info.Address2 = model.Address2;
            info.City = model.City;
            info.State = model.State;
            info.ZipCode = model.ZipCode;
            info.Country = model.Country;

            var partList = new List<WarrantyPart>();            
            foreach (var partInfo in model.PartsRequested ?? [])
            {                
                var part = new WarrantyPart() { Name = partInfo.Name, Quantity = partInfo.Quantity };
                partList.Add(part);
            }
            info.PartsRequested = partList;

            info.ProblemDescription = model.ProblemDescription;
            info.TermsAcceptance = model.TermsAcceptance;
            return info;
        }
    }
}
