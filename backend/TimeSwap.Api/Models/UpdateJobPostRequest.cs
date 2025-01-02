namespace TimeSwap.Api.Models
{
    public class UpdateJobPostRequest : JobPostCommand
    {
        public Guid Id { get; set; }
    }
}
