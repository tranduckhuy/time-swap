using MediatR;
using TimeSwap.Application.Industries.Responses;

namespace TimeSwap.Application.Industries.Queries
{
    public class GetIndustryByIdQuery : IRequest<IndustryResponse>
    {
        public int Id { get; set; }

        public GetIndustryByIdQuery(int id)
        {
            Id = id;
        }
    }
}
