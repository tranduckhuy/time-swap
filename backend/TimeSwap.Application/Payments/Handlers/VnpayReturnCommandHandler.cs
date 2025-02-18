using MediatR;
using Microsoft.Extensions.Options;
using TimeSwap.Application.Configurations.Payments;
using TimeSwap.Application.Configurations.Payments.Responses;
using TimeSwap.Application.Exceptions.Auth;
using TimeSwap.Application.Exceptions.Payments;
using TimeSwap.Application.Payments.Commands;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Payments.Handlers
{
    public class VnpayReturnCommandHandler : IRequestHandler<VnpayReturnCommand, string>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ITransactionLogRepository _transactionLogRepository;
        private readonly IUserRepository _userRepository;
        private readonly VnPayConfig _vnPayConfig;

        public VnpayReturnCommandHandler(
            IPaymentRepository paymentRepository,
            IOptions<VnPayConfig> vnPayConfig,
            ITransactionLogRepository transactionLogRepository,
            IUserRepository userRepository)
        {
            _paymentRepository = paymentRepository;
            _vnPayConfig = vnPayConfig.Value;
            _transactionLogRepository = transactionLogRepository;
            _userRepository = userRepository;
        }

        public async Task<string> Handle(VnpayReturnCommand request, CancellationToken cancellationToken)
        {
            var response = new VnPayOneTimePaymentCreateLinkResponse
            {
                vnp_TmnCode = request.vnp_TmnCode,
                vnp_Amount = request.vnp_Amount,
                vnp_BankCode = request.vnp_BankCode,
                vnp_BankTranNo = request.vnp_BankTranNo,
                vnp_CardType = request.vnp_CardType,
                vnp_PayDate = request.vnp_PayDate,
                vnp_OrderInfo = request.vnp_OrderInfo,
                vnp_TransactionNo = request.vnp_TransactionNo,
                vnp_ResponseCode = request.vnp_ResponseCode,
                vnp_TransactionStatus = request.vnp_TransactionStatus,
                vnp_TxnRef = request.vnp_TxnRef,
                vnp_SecureHash = request.vnp_SecureHash
            };

            if (!response.isValidSignature(_vnPayConfig.HashSecret))
            {
                throw new InvalidSignatureException();
            }

            var payment = await _paymentRepository.GetByIdAsync(Guid.Parse(response.vnp_TxnRef))
                          ?? throw new PaymentNotExistsException();

            bool isSuccess = response.vnp_ResponseCode == "00";
            payment.PaymentStatus = isSuccess ? PaymentStatus.Paid : PaymentStatus.Failed;

            await _paymentRepository.UpdateAsync(payment);

            if (!isSuccess)
            {
                await LogTransaction(payment, TransactionEvent.PaymentFailed);
                throw GetPaymentException(response.vnp_ResponseCode);
            }

            var user = await _userRepository.GetByIdAsync(payment.UserId)
                       ?? throw new UserNotExistsException();

            user.Balance += response.vnp_Amount / 100;
            await _userRepository.UpdateAsync(user);

            await LogTransaction(payment, TransactionEvent.PaymentCompleted);
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

        private Exception GetPaymentException(string responseCode) =>
            responseCode switch
            {
                "07" => new TransactionSuspectedOfFraudException(),
                "09" => new AccountNotRegisteredForInternetBankingException(),
                "10" => new CardAccountAuthenticationFailedMoreThan3TimesException(),
                "11" => new PaymentTimeoutException(),
                "12" => new CardAccountIsLockedException(),
                "13" => new IncorrectTransactionAuthenticationPasswordException(),
                "24" => new TransactionCanceledByCustomerException(),
                "51" => new InsufficientAccountBalanceException(),
                "65" => new TransactionLimitExceededException(),
                "75" => new BankIsUnderMaintenanceException(),
                "79" => new IncorrectPaymentPasswordExceededException(),
                "99" => new UndefinedErrorException(),
                _ => new PaymentFailedException(),
            };
    }
}
