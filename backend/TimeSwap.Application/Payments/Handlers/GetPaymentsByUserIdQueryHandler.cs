using MediatR;
using TimeSwap.Application.Exceptions.Payments;
using TimeSwap.Application.Mappings;
using TimeSwap.Application.Payments.Queries;
using TimeSwap.Application.Payments.Responses;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Domain.Specs;

namespace TimeSwap.Application.Payments.Handlers
{
    public class GetPaymentsByUserIdQueryHandler(IPaymentRepository paymentRepository) : IRequestHandler<GetPaymentsByUserIdQuery, Pagination<PaymentDetailResponse>>
    {
        private readonly IPaymentRepository _paymentRepository = paymentRepository;

        public async Task<Pagination<PaymentDetailResponse>> Handle(GetPaymentsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var paginationResult = await _paymentRepository
                .GetPaymentsByUserIdAsync(
                    request.UserId,
                    request.PaymentStatus,
                    request.DateFilter,
                    request.PageIndex,
                    request.PageSize
                );

            if (paginationResult.Data.Count == 0)
            {
                throw new PaymentNotFoundByUserIdException();
            }

            return AppMapper<CoreMappingProfile>.Mapper.Map<Pagination<PaymentDetailResponse>>(paginationResult);
        }
    }
}
