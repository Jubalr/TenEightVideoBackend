using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using TenEightVideo.Web.Configuration;
using TenEightVideo.Web.Updates;

namespace TenEightVideo.Web.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdatesController : ApiControllerBase
    {
        private static readonly Regex AppKeyPattern = new(@"^\d{4,20}$", RegexOptions.Compiled);

        private readonly IUpdateChecker _updateChecker;

        public UpdatesController(IUpdateChecker updateChecker, IOptions<ApiSettings> apiSettingOptions, ILogger<UpdatesController> logger)
            : base(apiSettingOptions, logger)
        {
            _updateChecker = updateChecker;
        }

        [HttpGet("check")]
        [EnableRateLimiting("updates")]
        [Produces("text/plain")]
        public IActionResult Check([FromQuery] string? appKey)
        {
            try
            {
                if (string.IsNullOrEmpty(appKey) || !AppKeyPattern.IsMatch(appKey))
                {
                    Logger.LogWarning("Invalid appKey: {AppKey}", appKey);
                    return BadRequest("ERROR: Invalid request");
                }

                var version = _updateChecker.GetLatestVersion(appKey);
                Logger.LogInformation("Update check OK: {AppKey}={Version}", appKey, version);
                return Ok(version);
            }
            catch (KeyNotFoundException)
            {
                Logger.LogWarning("Unknown application: {AppKey}", appKey);
                return NotFound("ERROR: Unknown application");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error checking updates for {AppKey}", appKey);
                return StatusCode(StatusCodes.Status500InternalServerError, "ERROR: Internal server error");
            }
        }
    }
}
