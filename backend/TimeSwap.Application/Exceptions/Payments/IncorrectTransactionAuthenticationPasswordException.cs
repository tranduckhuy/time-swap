using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Payments
{
    public class IncorrectTransactionAuthenticationPasswordException : AppException
    {
        public IncorrectTransactionAuthenticationPasswordException() : base(StatusCode.IncorrectTransactionAuthenticationPassword)
        {
        }
    }
}
