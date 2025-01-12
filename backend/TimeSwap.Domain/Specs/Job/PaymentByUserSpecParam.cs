using TimeSwap.Shared.Constants;

namespace TimeSwap.Domain.Specs.Job
{
    public class PaymentByUserSpecParam : BaseSpecParam
    {
        public PaymentStatus? PaymentStatus { get; set; }

        public DateFilter? DateFilter { get; set; }
    }
}
