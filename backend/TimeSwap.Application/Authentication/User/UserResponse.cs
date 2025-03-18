using TimeSwap.Application.Categories.Responses;
using TimeSwap.Application.Industries.Responses;
using TimeSwap.Application.Location.Responses;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Authentication.User
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public List<string> Role { get; set; } = [];
        public CityResponse? City { get; set; } 
        public WardResponse? Ward { get; set; }
        public string AvatarUrl { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; }
        public DateTime? SubscriptionExpiryDate { get; set; }
        public IEnumerable<string>? EducationHistory { get; set; }
        public CategoryResponse? MajorCategory { get; set; }
        public IndustryResponse? MajorIndustry { get; set; }
    }
}
