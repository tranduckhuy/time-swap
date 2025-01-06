using MediatR;

namespace TimeSwap.Application.Industries.Commands
{
    public class UpdateIndustryCommand : IRequest<Unit>
    {
        public int IndustryId { get; set; }

        public string IndustryName { get; set; } = string.Empty;
    }
}
