using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Api.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly IMediator _mediator;

        protected BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private BadRequestObjectResult ValidateRequest<TRequest, TResponse>()
            where TRequest : class, IRequest<TResponse>
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(x => x.Errors.Select(e => e.ErrorMessage))
                    .ToList();

                var statusCode = Shared.Constants.StatusCode.ModelInvalid;

                return BadRequest(new ApiResponse<TRequest>
                {
                    StatusCode = (int)statusCode,
                    Message = ResponseMessages.GetMessage(statusCode),
                    Errors = errors
                });
            }

            return null!;
        }

        protected async Task<IActionResult> ExecuteAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class, IRequest<TResponse>
        {
            var validationResult = ValidateRequest<TRequest, TResponse>();
            if (validationResult != null)
            {
                return validationResult;
            }

            try
            {
                var response = await _mediator.Send(request);

                var successCode = Shared.Constants.StatusCode.RequestProcessedSuccessfully;

                return Ok(
                    new ApiResponse<TResponse>
                    {
                        StatusCode = (int)successCode,
                        Data = response,
                        Message = ResponseMessages.GetMessage(successCode)
                    }
                );
            }
            catch (Exception ex)
            {
                return HandleError<TResponse>(ex);
            }
        }

        private IActionResult HandleError<TResponse>(Exception ex)
        {
            // Handle custom exceptions
            if (ex is AppException exception)
            {
                return Ok(new ApiResponse<TResponse>
                {
                    StatusCode = (int)exception.StatusCode,
                    Message = ex.Message,
                });
            }

            var statusCode = (int)HttpStatusCode.InternalServerError;

            return StatusCode(statusCode, new ApiResponse<TResponse>
            {
                StatusCode = statusCode,
                Message = ex.Message,
                Errors = ex.StackTrace?.Split("\n")
            });
        }
    }
}
