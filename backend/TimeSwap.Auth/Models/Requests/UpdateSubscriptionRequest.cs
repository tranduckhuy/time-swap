using System.Text.Json.Serialization;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Auth.Models.Requests
{
    public class UpdateSubscriptionRequest
    {
        [JsonRequired]
        public SubscriptionPlan SubscriptionPlan { get; set; }
    }
}
