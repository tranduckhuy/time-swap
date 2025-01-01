using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Categories
{
    public class InvalidCategoryInIndustryException : AppException
    {
        public InvalidCategoryInIndustryException() : base(StatusCode.InvalidCategoryInIndustryException) { }
    }
}
