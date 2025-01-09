using TimeSwap.Domain.Specs;

namespace TimeSwap.Domain.Specs.Job
{
    public class JobPostByUserSpecParam : BaseSpecParam
    {
        public Guid UserId { get; set; }
    }
}
