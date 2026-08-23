using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace my_auth_api_demo.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class PingController : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "RequireAdmin")]
        public IActionResult Get() => Ok("pong");
    }
}
