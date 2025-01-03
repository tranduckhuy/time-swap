using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Authentication
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string FullLocation { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; }
        public DateTime SubscriptionExpiryDate { get; set; }
    }
}
