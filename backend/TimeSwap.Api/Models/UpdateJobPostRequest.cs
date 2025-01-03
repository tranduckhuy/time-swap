using System.Text.Json.Serialization;

namespace TimeSwap.Api.Models
{
    public class UpdateJobPostRequest : JobPostCommand
    {
        [JsonRequired]
        public Guid Id { get; set; }
    }
}
