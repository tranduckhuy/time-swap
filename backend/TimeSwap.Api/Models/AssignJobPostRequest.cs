namespace TimeSwap.Api.Models
{
    public class AssignJobPostRequest
    {
        public Guid JobPostId { get; set; }
        public Guid UserAppliedId { get; set; }
    }
}
