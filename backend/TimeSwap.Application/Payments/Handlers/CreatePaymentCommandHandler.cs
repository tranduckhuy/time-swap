using MediatR;
using Microsoft.Extensions.Options;
using TimeSwap.Application.Payments.Commands;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Application.Exceptions.Auth;
using TimeSwap.Application.Exceptions.PaymentMethods;
using TimeSwap.Application.Mappings;
using TimeSwap.Shared.Constants;
using TimeSwap.Application.Configurations.Payments;
using TimeSwap.Application.Configurations.Payments.Requests;

namespace TimeSwap.Application.Payments.Handlers
{

    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, string>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ITransactionLogRepository _transactionLogRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly VnPayConfig _vnPayConfig;

        public CreatePaymentCommandHandler(
            IPaymentRepository paymentRepository,
            ITransactionLogRepository transactionLogRepository,
            IOptions<VnPayConfig> vnPayConfig,
            IUserRepository userRepository,
            IPaymentMethodRepository paymentMethodRepository)
        {
            _paymentRepository = paymentRepository;
            _vnPayConfig = vnPayConfig.Value;
            _transactionLogRepository = transactionLogRepository;
            _userRepository = userRepository;
            _paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<string> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)

        {
            var userId = await _userRepository.GetByIdAsync(request.UserId) ?? throw new UserNotExistsException();
            var paymentMethodId = await _paymentMethodRepository.GetByIdAsync(request.PaymentMethodId) ?? throw new PaymentMethodNotExistsException();

            var payment = AppMapper<CoreMappingProfile>.Mapper.Map<Payment>(request);

            await _paymentRepository.AddAsync(payment);

            var vnPayPayment = new VnPayOneTimePaymentRequest
            {
                vnp_Version = _vnPayConfig.Version,
                vnp_TmnCode = _vnPayConfig.TmnCode,
                vnp_Amount = request.Amount * 100,
                vnp_OrderInfo = request.PaymentContent ?? string.Empty,
                vnp_OrderType = "nap_tien",
                vnp_ReturnUrl = _vnPayConfig.ReturnUrl,
                vnp_TxnRef = payment.Id.ToString(),
                vnp_IpAddr = request.IpAddress,
                vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss"),
                vnp_ExpireDate = DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss"),
                vnp_CurrCode = "VND",
                vnp_Locale = "vn"
            };

            var paymentUrl = vnPayPayment.GetLink(_vnPayConfig.PaymentUrl, _vnPayConfig.HashSecret);

            var transactionLog = AppMapper<CoreMappingProfile>.Mapper.Map<TransactionLog>(payment);
            transactionLog.Id = Guid.NewGuid();
            transactionLog.PaymentId = payment.Id;
            transactionLog.TransactionEvent = TransactionEvent.PaymentInitiated;

            await _transactionLogRepository.AddAsync(transactionLog);

            return paymentUrl;
        }
    }
}
