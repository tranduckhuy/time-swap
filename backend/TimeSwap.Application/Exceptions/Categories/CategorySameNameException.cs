using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Categories
{
    public class CategorySameNameException : AppException
    {
        public CategorySameNameException() : base(StatusCode.CategorySameName) { }
    }
}
