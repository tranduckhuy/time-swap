using System.Linq.Expressions;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.Job;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Infrastructure.Specifications
{
    public class PaymentByUserSpecification : ISpecification<Payment>
    {
        public Expression<Func<Payment, bool>> Criteria { get; private set; }

        public Func<IQueryable<Payment>, IOrderedQueryable<Payment>>? OrderBy { get; private set; }

        public Func<IQueryable<Payment>, IOrderedQueryable<Payment>>? OrderByDescending { get; private set; }

        public List<Expression<Func<Payment, object>>> Includes { get; private set; } = [];

        public Func<IQueryable<Payment>, IQueryable<Payment>>? Selector { get; private set; }

        public int Skip { get; private set; }

        public int Take { get; private set; }


        public PaymentByUserSpecification(PaymentByUserSpecParam param, Guid userId)
        {
            PaymentStatus? paymentStatus = param.PaymentStatus;

            DateTime? dateFilter = param.DateFilter switch
            {
                DateFilter.Today => DateTime.UtcNow.Date,
                DateFilter.Yesterday => DateTime.UtcNow.Date.AddDays(-1),
                DateFilter.Last7Days => DateTime.UtcNow.Date.AddDays(-7),
                DateFilter.Last30Days => DateTime.UtcNow.Date.AddDays(-30),
                _ => null,
            };

            Criteria = payment =>
                payment.UserId == userId &&
                (paymentStatus == null || payment.PaymentStatus == paymentStatus) &&
                (dateFilter == null || payment.CreatedAt >= dateFilter);

            OrderBy = q => q.OrderBy(p => p.CreatedAt);

            Skip = (param.PageIndex - 1) * param.PageSize;
            Take = param.PageSize;

            Includes.Add(p => p.User);
            Includes.Add(p => p.PaymentMethod);
        }
    }
}
