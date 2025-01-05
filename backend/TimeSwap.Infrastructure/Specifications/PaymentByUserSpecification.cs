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

            DateTime? startDate = null;
            DateTime? endDate = null;

            switch (dateFilter)
            {
                case 0:
                    startDate = DateTime.UtcNow.Date;
                    endDate = DateTime.UtcNow.Date.AddDays(1);
                    break;
                case 1:
                    startDate = DateTime.UtcNow.Date.AddDays(-1);
                    endDate = DateTime.UtcNow.Date;
                    break;
                case 2:
                    startDate = DateTime.UtcNow.AddDays(-7);
                    endDate = DateTime.UtcNow;
                    break;
                case 3:
                    startDate = DateTime.UtcNow.AddDays(-30);
                    endDate = DateTime.UtcNow;
                    break;
            }

            Criteria = payment =>
                payment.UserId == userId &&
                (string.IsNullOrEmpty(paymentStatus) || payment.PaymentStatus == parsedStatus) &&
                (startDate == null || (payment.CreatedAt >= startDate && payment.CreatedAt < endDate));

            OrderBy = q => q.OrderBy(p => p.CreatedAt);
            Skip = (pageIndex - 1) * pageSize;
            Take = pageSize;

            Includes.Add(p => p.User);
            Includes.Add(p => p.PaymentMethod);
        }
    }
}
