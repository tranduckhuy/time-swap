using MediatR;
using Microsoft.AspNetCore.Mvc;
using TimeSwap.Application.Categories.Commands;
using TimeSwap.Application.Categories.Queries;
using TimeSwap.Application.Categories.Responses;
using TimeSwap.Shared;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Api.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : BaseController
    {
        public CategoryController(IMediator mediator) : base(mediator) { }


        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var query = new GetAllCategoriesQuery();
            return await ExecuteAsync<GetAllCategoriesQuery, List<CategoryResponse>>(query);
        }

        [HttpPut("{categoryId}")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryCommand command, int categoryId = 1)
        {
            if (command == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Data = null,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = new List<string> { "Request body cannot be null" }
                });
            }
            command.CategoryId = categoryId;
            return await ExecuteAsync<UpdateCategoryCommand, bool>(command);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
        {
            return await ExecuteAsync<CreateCategoryCommand, int>(command);
        }

    }
}
