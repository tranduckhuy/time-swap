namespace TimeSwap.Application.JobPosts.Responses
{
    public class JobPostDetailResponse : JobPostResponseCommon
    {
        public int TotalApplicants { get; set; }

        public IEnumerable<JobPostResponse> RelatedJobPosts { get; set; } = [];
        public string? OwnerEmail { get; set; } = string.Empty;
        public string? OwnerLocation { get; set; } = string.Empty;
    }
}
