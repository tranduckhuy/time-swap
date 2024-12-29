using Microsoft.AspNetCore.Mvc;
using System.Net;
using TimeSwap.Application.Mappings;
using TimeSwap.Auth.Mappings;
using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Auth.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult CheckModelStateValidity()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(x => x.Errors.Select(e => e.ErrorMessage))
                    .ToList();

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

        protected async Task<IActionResult> HandleRequestAsync<TRequest, TDto>(TRequest request, Func<TDto, Task<StatusCode>> serviceCall)
        {
            var badRequestResponse = CheckModelStateValidity();
            if (badRequestResponse != null)
            {
                return badRequestResponse;
            }

            try
            {
                var dto = AppMapper<AuthMappingProfile>.Mapper.Map<TDto>(request);
                var statusCode = await serviceCall(dto);

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

        protected async Task<IActionResult> HandleRequestWithResponseAsync<TRequest, TDto, TResponse>
            (TRequest request, Func<TDto, Task<(StatusCode, TResponse)>> serviceCall)
        {
            var badRequestResponse = CheckModelStateValidity();
            if (badRequestResponse != null)
            {
                return badRequestResponse;
            }

            try
            {
                var dto = AppMapper<AuthMappingProfile>.Mapper.Map<TDto>(request);
                var (statusCode, response) = await serviceCall(dto);

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

            return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<object>
            {
                StatusCode = (int)Shared.Constants.StatusCode.UserAuthenticationFailed,
                Message = ex.Message,
                Errors = ex.StackTrace?.Split("\n")
            });
        }
    }
}
