using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TimeSwap.Auth.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult TestAction() => Ok("Test action");

        [HttpGet("open")]
        [AllowAnonymous]
        public IActionResult OpenAction() => Ok("Open action");

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminAction() => Ok("Admin action");

        [HttpGet("user")]
        public IActionResult UserAction() => Ok("User deploy action duoc diiii hehehe!");
    }
}
