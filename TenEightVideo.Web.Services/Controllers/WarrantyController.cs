using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using TenEightVideo.Web.Configuration;
using TenEightVideo.Web.Data;
using TenEightVideo.Web.Mail;
using TenEightVideo.Web.Services.Models;
using TenEightVideo.Web.Warranty;

namespace TenEightVideo.Web.Services.Controllers
{
    public class WarrantyController : ApiControllerBase
    {
        private readonly IWarrantyRequestManager _warrantyRequestManager;
        private readonly IMailManager _mailManager;

        public WarrantyController(IWarrantyRequestManager warrantyRequestManager, IMailManager mailManager, IOptions<ApiSettings> apiSettingOptions, ILogger<WarrantyController> logger) 
            : base(apiSettingOptions, logger)
        {
            _warrantyRequestManager = warrantyRequestManager;
            _mailManager = mailManager;
        }

        [HttpPost("CreateWarrantyPartsRequest")]
        public IActionResult CreateWarrantyPartsRequest([FromBody] WarrantyModel model)
        {
            try
            {
                var request = new WarrantyRequest();
                request.WarrantyRequestParts = new List<WarrantyRequestPart>();
                ModelMapper.Map(model, request);
                _warrantyRequestManager.CreateRequest(request);
                var info = ModelMapper.Map(model);
                info.RequestId = request.Id;

                var sender = new MailAddress(ApiSettings.ServerEmailAddress!);
                var recipient = new MailAddress(ApiSettings.SalesEmailAddress!);
                _mailManager.SendWarrantyNotification(sender, recipient, info);
                return Ok("Warranty parts notification email sent successfully.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error sending warranty parts notification email.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error sending warranty parts notification email.");
            }
        }

    }
}
