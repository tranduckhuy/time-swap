using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TimeSwap.Api.Mapping;
using TimeSwap.Api.Models;
using TimeSwap.Application.Categories.Commands;
using TimeSwap.Application.Categories.Queries;
using TimeSwap.Application.Categories.Responses;
using TimeSwap.Application.Mappings;
using TimeSwap.Shared;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Api.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : BaseController<CategoryController>
    {
        public CategoryController(
            IMediator mediator,
            ILogger<CategoryController> logger
        ) : base(mediator, logger) { }


        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<CategoryResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllCategories()
        {
            var query = new GetAllCategoriesQuery();
            return await ExecuteAsync<GetAllCategoriesQuery, List<CategoryResponse>>(query);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(ApiResponse<int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["The request body does not contain required fields"]
                });
            }
            var command = AppMapper<ModelMapping>.Mapper.Map<CreateCategoryCommand>(request);
            return await ExecuteAsync<CreateCategoryCommand, int>(command);
        }

        [HttpPut("{categoryId}")]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequest request, int categoryId)
        {
            if (request == null || request.CategoryId != categoryId)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["The request body is invalid or does not match the category ID in the route."]
                });
            }

            var command = AppMapper<ModelMapping>.Mapper.Map<UpdateCategoryCommand>(request);
            return await ExecuteAsync<UpdateCategoryCommand, Unit>(command);
        }
    }
}
