using MediatR;

namespace TimeSwap.Application.JobPosts.Commands
{
    public class AssignJobCommand : IRequest<Unit>
    {
        public Guid OwnerId { get; set; }

        public Guid JobPostId { get; set; }

        public Guid UserAppliedId { get; set; }
    }
}
