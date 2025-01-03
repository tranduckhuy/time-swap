using System.Text.Json.Serialization;

namespace TimeSwap.Api.Models
{
    public class CreateJobApplicantRequest
    {
        [JsonRequired]
        public Guid JobPostId { get; set; }

        [JsonRequired]
        public Guid UserAppliedId { get; set; }
    }
}
