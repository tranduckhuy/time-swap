using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TimeSwap.Application.Authentication;
using TimeSwap.Application.Authentication.Interfaces;
using TimeSwap.Shared;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Auth.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : BaseController<UserController>
    {
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService) : base(logger)
        {
            _userService = userService;
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetUserProfileAsync()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.InvalidToken,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.InvalidToken),
                    Errors = ["User id does not exist in the claims. Please login again."]
                });
            }

            return await HandleRequestWithResponseAsync<Guid, Guid, UserResponse>(Guid.Parse(userId), _userService.GetUserProfileAsync);
        }
    }
}
