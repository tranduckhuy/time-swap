namespace TimeSwap.Application.JobApplicants.Responses
{
    public class JobApplicantResponse
    {
        public Guid UserId { get; set; }
        public Guid JobPostId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime AppliedAt { get; set; }
    }
}
