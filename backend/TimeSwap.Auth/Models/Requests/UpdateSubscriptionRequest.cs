using System.ComponentModel.DataAnnotations;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Auth.Models.Requests
{
    public class UpdateSubscriptionRequest
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public SubscriptionPlan SubscriptionPlan { get; set; }
    }
}
