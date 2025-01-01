using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Categories
{
    public class CategoryNotFoundByIndustryIdException : AppException
    {
        public CategoryNotFoundByIndustryIdException() : base(StatusCode.CategoryNotFoundByIndustryId) { }
    }
}
