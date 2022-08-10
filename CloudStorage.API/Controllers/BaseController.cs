using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CloudStorage.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        internal string UserId => User.Identity!.IsAuthenticated
            ? User.FindFirst(ClaimTypes.NameIdentifier)!.Value : string.Empty;

    }
}
