namespace TimeSwap.Api.Models
{
    public class CreateJobApplicantRequest
    {
        public Guid JobPostId { get; set; }
        public Guid UserAppliedId { get; set; }
    }
}
