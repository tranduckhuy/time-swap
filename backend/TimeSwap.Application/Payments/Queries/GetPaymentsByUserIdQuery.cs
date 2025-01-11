using MediatR;
using TimeSwap.Application.Payments.Responses;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.Job;

namespace TimeSwap.Application.Payments.Queries
{
    public class GetPaymentsByUserIdQuery : IRequest<Pagination<PaymentDetailResponse>>
    {
        public PaymentByUserSpecParam SpecParam { get; }
        public Guid UserId { get; }

        public GetPaymentsByUserIdQuery(PaymentByUserSpecParam specParam, Guid userId)
        {
            SpecParam = specParam;
            UserId = userId;
        }
    }
}
