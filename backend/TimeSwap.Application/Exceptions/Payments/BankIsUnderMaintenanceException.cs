using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Payments
{
    public class BankIsUnderMaintenanceException : AppException
    {
        public BankIsUnderMaintenanceException() : base(StatusCode.BankIsUnderMaintenance)
        {
        }
    }
}
