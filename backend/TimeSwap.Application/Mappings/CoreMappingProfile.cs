using AutoMapper;
using TimeSwap.Application.Responses;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Specs;

namespace TimeSwap.Application.Mappings
{
    public class CoreMappingProfile : Profile
    {
        public CoreMappingProfile() {
            CreateMap<JobPost, JobPostResponse>().ReverseMap();
            CreateMap<Pagination<JobPost>, Pagination<JobPostResponse>>().ReverseMap();
        }
    }
}
