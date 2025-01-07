namespace TimeSwap.Application.Authentication.User
{
    public class UpdateUserProfileRequestDto
    {
        public Guid UserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Description { get; set; }

        public string? AvatarUrl { get; set; }

        public string? CityId { get; set; }

        public string? WardId { get; set; }

        public IList<string>? EducationHistory { get; set; }

        public int MajorCategoryId { get; set; }

        public int MajorIndustryId { get; set; }
    }
}
