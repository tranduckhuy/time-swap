using MediatR;
using TimeSwap.Application.Payments.Responses;

namespace TimeSwap.Application.Payments.Queries
{
    public class GetPaymentsByUserIdQuery : IRequest<List<PaymentDetailResponse>>
    {
        public Guid UserId { get; set; }

        public GetPaymentsByUserIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
