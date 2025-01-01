using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Categories
{
    public class CategoryNotFoundException : AppException
    {
        public CategoryNotFoundException() : base(StatusCode.CategoryNotFound) { }
    }
}
