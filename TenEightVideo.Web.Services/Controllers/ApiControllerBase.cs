using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TenEightVideo.Web.Configuration;

namespace TenEightVideo.Web.Services.Controllers
{
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        public ApiControllerBase(IOptions<ApiSettings> apiSettingOptions, ILogger logger)
        {
            ApiSettings = apiSettingOptions.Value;
            Logger = logger;
        }

        protected ApiSettings ApiSettings { get; }
        protected ILogger Logger { get; }
    }
}
