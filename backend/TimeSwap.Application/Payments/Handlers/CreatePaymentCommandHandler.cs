using MediatR;
using Microsoft.Extensions.Options;
using TimeSwap.Application.Payments.Commands;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Application.Exceptions.Auth;
using TimeSwap.Application.Exceptions.PaymentMethods;
using TimeSwap.Application.Mappings;
using TimeSwap.Application.Configurations.Payments;
using TimeSwap.Application.Configurations.Payments.Requests;
using Net.payOS;
using Net.payOS.Types;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Payments.Handlers
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, string>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ITransactionLogRepository _transactionLogRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly VnPayConfig _vnPayConfig;
        private readonly PayOS _payOS;
        private readonly PayOSConfig _payOSConfig;

        public CreatePaymentCommandHandler(
            IPaymentRepository paymentRepository,
            ITransactionLogRepository transactionLogRepository,
            IUserRepository userRepository,
            IPaymentMethodRepository paymentMethodRepository,
            IOptions<VnPayConfig> vnPayConfig,
            IOptions<PayOSConfig> payOSConfig,
            PayOS payOS)
        {
            _paymentRepository = paymentRepository;
            _vnPayConfig = vnPayConfig.Value;
            _payOSConfig = payOSConfig.Value;
            _payOS = payOS;
            _transactionLogRepository = transactionLogRepository;
            _userRepository = userRepository;
            _paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<string> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            _ = await _userRepository.GetByIdAsync(request.UserId) ?? throw new UserNotExistsException();
            _ = await _paymentMethodRepository.GetByIdAsync(request.PaymentMethodId) ?? throw new PaymentMethodNotExistsException();

            var payment = AppMapper<CoreMappingProfile>.Mapper.Map<Payment>(request);

            string paymentUrl = string.Empty;

            if (request.PaymentMethodId == 1)
            {
                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                ItemData item = new ItemData(request.PaymentContent ?? "Nap tien tai khoan", 1, (int)request.Amount);
                List<ItemData> items = new List<ItemData> { item };

                var paymentData = new PaymentData(
                    orderCode,
                    (int)request.Amount,
                    "Nap tien tai khoan",
                    items,
                    _payOSConfig.CancelUrl,
                    _payOSConfig.ReturnUrl
                );

                CreatePaymentResult createPaymentResult = await _payOS.createPaymentLink(paymentData);

                paymentUrl = createPaymentResult.checkoutUrl;

                payment.Id = new Guid(createPaymentResult.paymentLinkId.ToString());
                await _paymentRepository.AddAsync(payment);

                var transactionLog = AppMapper<CoreMappingProfile>.Mapper.Map<TransactionLog>(payment);
                transactionLog.Id = Guid.NewGuid();
                transactionLog.PaymentId = payment.Id;
                transactionLog.TransactionEvent = TransactionEvent.PaymentInitiated;

                await _transactionLogRepository.AddAsync(transactionLog);
            }
            else if (request.PaymentMethodId == 2)
            {
                await _paymentRepository.AddAsync(payment);

                var vnPayPayment = new VnPayOneTimePaymentRequest
                {
                    vnp_Version = _vnPayConfig.Version,
                    vnp_TmnCode = _vnPayConfig.TmnCode,
                    vnp_Amount = (int)(request.Amount * 100),
                    vnp_OrderInfo = request.PaymentContent ?? string.Empty,
                    vnp_OrderType = "Nap_tien",
                    vnp_ReturnUrl = _vnPayConfig.ReturnUrl,
                    vnp_TxnRef = payment.Id.ToString(),
                    vnp_IpAddr = request.IpAddress,
                    vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    vnp_ExpireDate = DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss"),
                    vnp_CurrCode = "VND",
                    vnp_Locale = "vn"
                };

                paymentUrl = vnPayPayment.GetLink(_vnPayConfig.PaymentUrl, _vnPayConfig.HashSecret);

                var transactionLog = AppMapper<CoreMappingProfile>.Mapper.Map<TransactionLog>(payment);
                transactionLog.Id = Guid.NewGuid();
                transactionLog.PaymentId = payment.Id;
                transactionLog.TransactionEvent = TransactionEvent.PaymentInitiated;

                await _transactionLogRepository.AddAsync(transactionLog);
            }

            return paymentUrl;
        }
    }
}
