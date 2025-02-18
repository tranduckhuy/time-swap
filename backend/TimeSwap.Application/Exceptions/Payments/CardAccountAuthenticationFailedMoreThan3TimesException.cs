using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Payments
{
    public class CardAccountAuthenticationFailedMoreThan3TimesException : AppException
    {
        public CardAccountAuthenticationFailedMoreThan3TimesException() : base(StatusCode.CardAccountAuthenticationFailedMoreThan3Times) { }
    }
}
