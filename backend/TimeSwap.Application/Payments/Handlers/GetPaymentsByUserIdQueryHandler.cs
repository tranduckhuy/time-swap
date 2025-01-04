using MediatR;
using TimeSwap.Application.Exceptions.Payments;
using TimeSwap.Application.Mappings;
using TimeSwap.Application.Payments.Queries;
using TimeSwap.Application.Payments.Responses;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.Payments.Handlers
{
    public class GetPaymentsByUserIdQueryHandler : IRequestHandler<GetPaymentsByUserIdQuery, List<PaymentDetailResponse>>
    {
        private readonly IPaymentRepository _paymentRepository;

        public GetPaymentsByUserIdQueryHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<List<PaymentDetailResponse>> Handle(GetPaymentsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var payments = await _paymentRepository.GetPaymentsByUserIdAsync(request.UserId);

            if (!payments.Any())
            {
                throw new PaymentNotFoundByUserIdException();
            }

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<PaymentDetailResponse>>(payments);
        }
    }
}
