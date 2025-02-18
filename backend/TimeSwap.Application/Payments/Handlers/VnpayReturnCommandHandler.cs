using MediatR;
using Microsoft.Extensions.Options;
using TimeSwap.Application.Configurations.Payments;
using TimeSwap.Application.Configurations.Payments.Responses;
using TimeSwap.Application.Exceptions.Auth;
using TimeSwap.Application.Exceptions.Payments;
using TimeSwap.Application.Mappings;
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

        public VnpayReturnCommandHandler(IPaymentRepository paymentRepository, IOptions<VnPayConfig> vnPayConfig, ITransactionLogRepository transactionLogRepository, IUserRepository userRepository)
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

            switch (response.vnp_ResponseCode)
            {
                case "00":
                    break;
                case "07":
                    throw new TransactionSuspectedOfFraudException();
                case "09":
                    throw new AccountNotRegisteredForInternetBankingException();
                case "10":
                    throw new CardAccountAuthenticationFailedMoreThan3TimesException();
                case "11":
                    throw new PaymentTimeoutException();
                case "12":
                    throw new CardAccountIsLockedException();
                case "13":
                    throw new IncorrectTransactionAuthenticationPasswordException();
                case "24":
                    throw new TransactionCanceledByCustomerException();
                case "51":
                    throw new InsufficientAccountBalanceException();
                case "65":
                    throw new TransactionLimitExceededException();
                case "75":
                    throw new BankIsUnderMaintenanceException();
                case "79":
                    throw new IncorrectPaymentPasswordExceededException();
                case "99":
                    throw new UndefinedErrorException();
                default:
                    throw new PaymentFailedException();
            }

            var payment = await _paymentRepository.GetByIdAsync(Guid.Parse(response.vnp_TxnRef)) ?? throw new PaymentNotExistsException();

            payment.PaymentStatus = response.vnp_ResponseCode == "00" ? PaymentStatus.Paid : PaymentStatus.Failed;
            await _paymentRepository.UpdateAsync(payment);

            if (payment.PaymentStatus == PaymentStatus.Paid)
            {
                var user = await _userRepository.GetByIdAsync(payment.UserId) ?? throw new UserNotExistsException();

                user.Balance += response.vnp_Amount / 100;
                await _userRepository.UpdateAsync(user);

                var transactionLog = AppMapper<CoreMappingProfile>.Mapper.Map<TransactionLog>(payment);
                transactionLog.Id = Guid.NewGuid();
                transactionLog.PaymentId = payment.Id;
                transactionLog.TransactionEvent = TransactionEvent.PaymentCompleted;

                await _transactionLogRepository.AddAsync(transactionLog);
            }
            else
            {
                var transactionLog = AppMapper<CoreMappingProfile>.Mapper.Map<TransactionLog>(payment);
                transactionLog.Id = Guid.NewGuid();
                transactionLog.PaymentId = payment.Id;
                transactionLog.TransactionEvent = TransactionEvent.PaymentFailed;

                await _transactionLogRepository.AddAsync(transactionLog);
            }

            return payment.PaymentStatus == PaymentStatus.Paid
                ? ResponseMessages.GetMessage(StatusCode.PaymentSuccess)
                : ResponseMessages.GetMessage(StatusCode.PaymentFailed);
        }
    }
}
