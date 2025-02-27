using System.Text.Json.Serialization;

namespace TimeSwap.Domain.Specs.Job
{
    public class JobApplicantSpecParam : BaseSpecParam
    {
        [JsonIgnore]
        public Guid JobPostId { get; set; }

        public string? Search { get; set; }
        public string Sort { get; set; } = "AppliedAt";
        public int CategoryId { get; set; }
        public int IndustryId { get; set; }
        public string? CityId { get; set; }
        public string? WardId { get; set; }
    }
}
