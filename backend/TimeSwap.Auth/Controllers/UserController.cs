using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using TimeSwap.Application.Authentication.Interfaces;
using TimeSwap.Application.Authentication.User;
using TimeSwap.Application.JobPosts.Responses;
using TimeSwap.Application.Mappings;
using TimeSwap.Auth.Mappings;
using TimeSwap.Auth.Models.Requests;
using TimeSwap.Domain.Specs;
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
        [ProducesResponseType(typeof(ApiResponse<UserResponse>), (int)HttpStatusCode.OK)]
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

            return await HandleRequestWithResponseAsync(Guid.Parse(userId), _userService.GetUserProfileAsync);
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateUserProfileAsync([FromBody] UpdateUserProfileRequest request)
        {

            if (request == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["The request body does not contain required fields or invalid data"]
                });
            }

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

            var dto = AppMapper<AuthMappingProfile>.Mapper.Map<UpdateUserProfileRequestDto>(request);

            dto.UserId = Guid.Parse(userId);

            return await HandleRequestAsync(dto, _userService.UpdateUserProfileAsync);
        }
    }
}
