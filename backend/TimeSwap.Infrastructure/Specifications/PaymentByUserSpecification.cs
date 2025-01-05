using System.Linq.Expressions;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Specs;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Infrastructure.Specifications
{
    public class PaymentByUserSpecification : ISpecification<Payment>
    {
        public Expression<Func<Payment, bool>> Criteria { get; private set; }
        public Func<IQueryable<Payment>, IOrderedQueryable<Payment>>? OrderBy { get; private set; }
        public Func<IQueryable<Payment>, IOrderedQueryable<Payment>>? OrderByDescending { get; private set; }
        public List<Expression<Func<Payment, object>>> Includes { get; private set; } = [];
        public int Skip { get; private set; }
        public int Take { get; private set; }

        public PaymentByUserSpecification(Guid userId, string? paymentStatus, int dateFilter, int pageIndex = 1, int pageSize = 10)
        {
            PaymentStatus parsedStatus = PaymentStatus.Pending;

            if (!string.IsNullOrEmpty(paymentStatus))
            {
                Enum.TryParse(paymentStatus, true, out parsedStatus);
            }

            Criteria = payment =>
                payment.UserId == userId &&
                (string.IsNullOrEmpty(paymentStatus) || payment.PaymentStatus == parsedStatus) &&
                (dateFilter == 0 ? payment.CreatedAt >= DateTime.UtcNow.Date && payment.CreatedAt < DateTime.UtcNow.Date.AddDays(1) :
                 dateFilter == 1 ? payment.CreatedAt >= DateTime.UtcNow.Date.AddDays(-1) && payment.CreatedAt < DateTime.UtcNow.Date :
                 dateFilter == 2 ? payment.CreatedAt >= DateTime.UtcNow.AddDays(-7) && payment.CreatedAt < DateTime.UtcNow :
                 dateFilter != 3 || payment.CreatedAt >= DateTime.UtcNow.AddDays(-30) && payment.CreatedAt < DateTime.UtcNow);

            OrderBy = q => q.OrderBy(p => p.CreatedAt);

            Skip = (pageIndex - 1) * pageSize;
            Take = pageSize;

            Includes.Add(p => p.User);
            Includes.Add(p => p.PaymentMethod);
        }
    }
}
