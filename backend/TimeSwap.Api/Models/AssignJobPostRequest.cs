using System.Text.Json.Serialization;

namespace TimeSwap.Api.Models
{
    public class AssignJobPostRequest
    {
        [JsonRequired]
        public Guid JobPostId { get; set; }

        [JsonRequired]
        public Guid UserAppliedId { get; set; }
    }
}
