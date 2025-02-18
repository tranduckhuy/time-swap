using MediatR;
using TimeSwap.Application.Exceptions.Auth;
using TimeSwap.Application.Exceptions.Payments;
using TimeSwap.Application.Payments.Commands;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Payments.Handlers
{
    public class PayOSReturnCommandHandler : IRequestHandler<PayOSReturnCommand, string>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ITransactionLogRepository _transactionLogRepository;
        private readonly IUserRepository _userRepository;

        public PayOSReturnCommandHandler(
            IPaymentRepository paymentRepository,
            ITransactionLogRepository transactionLogRepository,
            IUserRepository userRepository)
        {
            _paymentRepository = paymentRepository;
            _transactionLogRepository = transactionLogRepository;
            _userRepository = userRepository;
        }

        public async Task<string> Handle(PayOSReturnCommand request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetByIdAsync(Guid.Parse(request.id))
                          ?? throw new PaymentNotExistsException();

            bool isSuccess = request.status == "PAID";
            bool isCancelled = request.status == "CANCELLED" || request.cancel;

            if (isCancelled)
            {
                payment.PaymentStatus = PaymentStatus.Failed;
                await _paymentRepository.UpdateAsync(payment);
                await LogTransaction(payment, TransactionEvent.PaymentFailed);
                throw new TransactionCanceledByCustomerException();
            }

            if (!isSuccess)
            {
                payment.PaymentStatus = PaymentStatus.Failed;
                await _paymentRepository.UpdateAsync(payment);
                await LogTransaction(payment, TransactionEvent.PaymentFailed);
                throw new PaymentFailedException();
            }

            if (payment.PaymentStatus != PaymentStatus.Paid)
            {
                payment.PaymentStatus = PaymentStatus.Paid;
                var user = await _userRepository.GetByIdAsync(payment.UserId)
                           ?? throw new UserNotExistsException();

                user.Balance += payment.Amount;
                await _userRepository.UpdateAsync(user);

                await LogTransaction(payment, TransactionEvent.PaymentCompleted);
            }

            return ResponseMessages.GetMessage(StatusCode.PaymentSuccess);
        }

        private async Task LogTransaction(Payment payment, TransactionEvent transactionEvent)
        {
            await _transactionLogRepository.AddAsync(new TransactionLog
            {
                Id = Guid.NewGuid(),
                PaymentId = payment.Id,
                TransactionEvent = transactionEvent,
                TransactionId = payment.TransactionId,
                CreatedAt = DateTime.UtcNow
            });
        }
    }
}
