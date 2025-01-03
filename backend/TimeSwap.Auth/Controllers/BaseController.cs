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
                (Shared.Constants.StatusCode StatusCode, TResponse Response) result;

                if (typeof(TDto).IsClass && !typeof(TDto).IsAbstract)
                {
                    var dto = AppMapper<AuthMappingProfile>.Mapper.Map<TDto>(request);
                    result = await serviceCall(dto);
                }
                else
                {
                    result = await serviceCall((TDto)(object)request!);
                }

                return Ok(new ApiResponse<TResponse>
                {
                    StatusCode = (int) result.StatusCode,
                    Message = ResponseMessages.GetMessage(result.StatusCode),
                    Data = result.Response
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
                Errors = ex.StackTrace?.Split("\n")
            });
        }
    }
}
