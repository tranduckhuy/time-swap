using System.ComponentModel.DataAnnotations;

namespace TimeSwap.Auth.Models.Requests
{
    public class UpdateUserProfileRequest
    {
        public string? FirstName { get; set; }
        
        public string? LastName { get; set; }

        [RegularExpression(@"^(0[3|5|7|8|9]\d{8}|(\+84)[3|5|7|8|9]\d{8})$",
            ErrorMessage = "Invalid phone number format")]
        public string? PhoneNumber { get; set; }

        public string? Description { get; set; }

        public string? AvatarUrl { get; set; } 

        public string? CityId { get; set; }

        public string? WardId { get; set; }
        
        public IList<string>? EducationHistory { get; set; }
        
        public int? MajorCategoryId { get; set; }

        public int? MajorIndustryId { get; set; }
    }
}
