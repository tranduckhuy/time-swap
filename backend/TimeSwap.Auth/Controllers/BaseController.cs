using Microsoft.AspNetCore.Mvc;
using System.Net;
using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Auth.Controllers
{
    [ApiController]
    public abstract class BaseController<TController> : ControllerBase
    {
        private readonly ILogger<BaseController<TController>> _logger;

        protected BaseController(ILogger<BaseController<TController>> logger)
        {
            _logger = logger;
        }

        protected IActionResult CheckModelStateValidity()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(x => x.Errors.Select(e => e.ErrorMessage))
                    .ToList();

                _logger.LogWarning("Request validation failed for {ControllerName}: {ErrorMessages}",
                        typeof(TController).Name,
                        string.Join(", ", errors));

                var statusCode = Shared.Constants.StatusCode.ModelInvalid;

                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)statusCode,
                    Message = ResponseMessages.GetMessage(statusCode),
                    Errors = errors
                });
            }

            return null!;
        }

        protected async Task<IActionResult> HandleRequestAsync<TRequest>(TRequest request, Func<TRequest, Task<StatusCode>> serviceCall)
        {
            var badRequestResponse = CheckModelStateValidity();
            if (badRequestResponse != null)
            {
                return badRequestResponse;
            }

            try
            {
                var statusCode = await serviceCall(request);

                return Ok(new ApiResponse<object>
                {
                    StatusCode = (int)statusCode,
                    Message = ResponseMessages.GetMessage(statusCode),
                });
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        protected async Task<IActionResult> HandleRequestWithResponseAsync<TRequest, TResponse>
            (TRequest request, Func<TRequest, Task<(StatusCode, TResponse)>> serviceCall)
        {
            var badRequestResponse = CheckModelStateValidity();
            if (badRequestResponse != null)
            {
                return badRequestResponse;
            }

            try
            {
                var (statusCode, response) = await serviceCall(request);

                return Ok(new ApiResponse<TResponse>
                {
                    StatusCode = (int)statusCode,
                    Message = ResponseMessages.GetMessage(statusCode),
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        private IActionResult HandleError(Exception ex)
        {
            if (ex is AuthException authException)
            {
                return Unauthorized(new ApiResponse<object>
                {
                    StatusCode = (int)authException.StatusCode,
                    Message = authException.Message,
                    Errors = authException.Errors
                });
            }

            if (ex is AppException appException)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)appException.StatusCode,
                    Message = appException.Message,
                    Errors = appException.Errors
                });
            }

            _logger.LogError(ex, "An error occurred while processing request for {ControllerName}", typeof(TController).Name);

            return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<object>
            {
                StatusCode = (int)Shared.Constants.StatusCode.UserAuthenticationFailed,
                Message = ex.Message,
                Errors = [ex.InnerException?.Message ?? "The system encountered an unexpected error while processing the request"]
            });
        }

        protected IActionResult UnauthorizedUserTokenResponse()
        {
            return Unauthorized(new ApiResponse<object>
            {
                StatusCode = (int)Shared.Constants.StatusCode.InvalidToken,
                Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.InvalidToken),
                Errors = ["User id does not exist in the claims. Please login again."]
            });
        }

        protected IActionResult NullRequestDataResponse()
        {
            return BadRequest(new ApiResponse<object>
            {
                StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                Errors = ["The request body does not contain required fields or invalid data"]
            });
        }
    }
}
