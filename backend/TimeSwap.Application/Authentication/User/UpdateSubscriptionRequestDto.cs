using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Authentication.User
{
    public class UpdateSubscriptionRequestDto
    {
        public Guid UserId { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; }
    }
}
