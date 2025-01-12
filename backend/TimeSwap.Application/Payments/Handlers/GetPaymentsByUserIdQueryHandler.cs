using MediatR;
using TimeSwap.Application.Mappings;
using TimeSwap.Application.Payments.Queries;
using TimeSwap.Application.Payments.Responses;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Domain.Specs;

namespace TimeSwap.Application.Payments.Handlers
{
    public class GetPaymentsByUserIdQueryHandler
         : IRequestHandler<GetPaymentsByUserIdQuery, Pagination<PaymentDetailResponse>>
    {
        private readonly IPaymentRepository _paymentRepository;

        public GetPaymentsByUserIdQueryHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<Pagination<PaymentDetailResponse>> Handle(GetPaymentsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var payments = await _paymentRepository.GetPaymentsByUserIdWithSpecAsync(request.SpecParam, request.UserId);

            return AppMapper<CoreMappingProfile>.Mapper.Map<Pagination<PaymentDetailResponse>>(payments);
        }
    }
}
