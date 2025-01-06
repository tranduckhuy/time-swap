using MediatR;
using TimeSwap.Application.Payments.Responses;
using TimeSwap.Domain.Specs;

namespace TimeSwap.Application.Payments.Queries
{
    public class GetPaymentsByUserIdQuery(Guid userId, string? paymentStatus = null, int dateFilter = 0, int pageIndex = 1, int pageSize = 10) : IRequest<Pagination<PaymentDetailResponse>>
    {
        public Guid UserId { get; init; } = userId;
        public string? PaymentStatus { get; init; } = paymentStatus;
        public int DateFilter { get; init; } = dateFilter;
        public int PageIndex { get; init; } = pageIndex;
        public int PageSize { get; init; } = pageSize;
    }
}
