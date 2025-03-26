using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using TimeSwap.Application.Authentication.Interfaces;
using TimeSwap.Application.Authentication.User;
using TimeSwap.Application.Mappings;
using TimeSwap.Auth.Mappings;
using TimeSwap.Auth.Models.Requests;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.User;
using TimeSwap.Shared;

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
        [ProducesResponseType(typeof(ApiResponse<UserResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserProfileAsync()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return UnauthorizedUserTokenResponse();
            }

            return await HandleRequestWithResponseAsync(Guid.Parse(userId), _userService.GetUserProfileAsync);
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateUserProfileAsync([FromBody] UpdateUserProfileRequest request)
        {

            if (request == null)
            {
                return NullRequestDataResponse();
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return UnauthorizedUserTokenResponse();
            }

            var dto = AppMapper<AuthMappingProfile>.Mapper.Map<UpdateUserProfileRequestDto>(request);

            dto.UserId = Guid.Parse(userId);

            return await HandleRequestAsync(dto, _userService.UpdateUserProfileAsync);
        }

        [HttpPut("subscription")]
        [Authorize]
        public async Task<IActionResult> UpdateSubscriptionAsync([FromBody] UpdateSubscriptionRequest request)
        {
            if (request == null)
            {
                return NullRequestDataResponse();
            }
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return UnauthorizedUserTokenResponse();
            }
            var dto = AppMapper<AuthMappingProfile>.Mapper.Map<UpdateSubscriptionRequestDto>(request);
            dto.UserId = Guid.Parse(userId);
            return await HandleRequestAsync(dto, _userService.UpdateSubscriptionAsync);
        }

        // Get user profile by id
        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserProfileByIdAsync(Guid userId)
        {
            return await HandleRequestWithResponseAsync(userId, _userService.GetUserProfileAsync);
        }

        // Get user list with pagination
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<Pagination<UserResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserListAsync([FromQuery] UserSpecParam request)
        {
            if (request == null)
            {
                return NullRequestDataResponse();
            }
            return await HandleRequestWithResponseAsync(request, _userService.GetUserListAsync);
        }
    }
}
