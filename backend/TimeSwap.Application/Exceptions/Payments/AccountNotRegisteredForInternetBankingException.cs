using TimeSwap.Shared.Constants;
using TimeSwap.Domain.Exceptions;

namespace TimeSwap.Application.Exceptions.Payments
{
    public class AccountNotRegisteredForInternetBankingException : AppException
    {
        public AccountNotRegisteredForInternetBankingException() : base(StatusCode.AccountNotRegisteredForInternetBanking) { }
    }
}
