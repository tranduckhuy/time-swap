using MediatR;

namespace TimeSwap.Application.Industries.Commands
{
    public class CreateIndustryCommand : IRequest<int>
    {
        public string IndustryName { get; set; } = string.Empty;
    }
}
