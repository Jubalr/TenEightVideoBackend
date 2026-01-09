using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using TenEightVideo.Web.Configuration;
using TenEightVideo.Web.Mail;

namespace TenEightVideo.Web.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ApiControllerBase
    {
        public MailController(IMailManager mailManager, IOptions<ApiSettings> appSettingOptions, ILogger<MailController> logger)
            : base(appSettingOptions, logger)
        {
            MailManager = mailManager;
        }

        protected IMailManager MailManager { get; }

        [HttpPost("SendTestEmail")]
        public IActionResult SendTestEmail()
        {
            try
            {
                var sender = new MailAddress(ApiSettings.ServerEmailAddress!);
                var recipient = new MailAddress(ApiSettings.ServiceEmailAddress!);
                MailManager.SendTestEmail(sender, recipient);
                return Ok("Test email sent successfully.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error sending test email.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error sending test email.");
            }
        }

        [HttpPost("SendContactNotificationEmail")]
        public IActionResult SendContactNotificationEmail([FromBody] ContactNotificationInfo info)
        {
            try
            {
                var sender = new MailAddress(ApiSettings.ServerEmailAddress!);
                var recipient = new MailAddress(ApiSettings.SalesEmailAddress!);
                MailManager.SendContactNotification(sender, recipient, info);
                return Ok("Contact notification email sent successfully.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error sending contact notification email.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error sending contact notification email.");
            }
        }

        [HttpPost("SendLeadMagnetEmail")]
        public IActionResult SendLeadMagnetEmail([FromBody] LeadMagnetInfo info)
        {
            try
            {
                var sender = new MailAddress(ApiSettings.ServerEmailAddress!);
                var recipient = new MailAddress(ApiSettings.SalesEmailAddress!);
                MailManager.SendLeadMagnetNotification(sender, recipient, info);
                return Ok("Contact notification email sent successfully.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error sending contact notification email.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error sending contact notification email.");
            }
        }
    }
}
