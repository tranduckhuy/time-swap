using AutoMapper;
using TimeSwap.Application.JobPosts.Responses;
using TimeSwap.Application.Location.Responses;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Specs;

namespace TimeSwap.Application.Mappings
{
    public class CoreMappingProfile : Profile
    {
        public CoreMappingProfile() {
            CreateMap<JobPost, JobPostResponse>().ReverseMap();
            CreateMap<Pagination<JobPost>, Pagination<JobPostResponse>>().ReverseMap();
            CreateMap<City, CityResponse>().ReverseMap();
            CreateMap<Ward, WardResponse>().ReverseMap();
        }
    }
}
