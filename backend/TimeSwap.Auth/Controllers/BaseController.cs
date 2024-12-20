using Microsoft.AspNetCore.Mvc;
using TimeSwap.Shared;

namespace TimeSwap.Auth.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult CheckModelStateValidity()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(x => x.Errors.Select(e => e.ErrorMessage))
                    .ToList();

                return BadRequest(new ApiResponse<object>
                {
                    Message = "Invalid request.",
                    Errors = errors
                });
            }

            return null!;
        }
    }
}
