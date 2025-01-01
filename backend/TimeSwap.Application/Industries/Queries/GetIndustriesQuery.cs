using MediatR;
using TimeSwap.Application.Industries.Responses;

namespace TimeSwap.Application.Industries.Queries
{
    public class GetIndustriesQuery : IRequest<List<IndustryResponse>>
    {
    }
}
