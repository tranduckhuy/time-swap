using AutoMapper;
using TimeSwap.Application.Categories.Responses;
using TimeSwap.Application.Industries.Responses;
using TimeSwap.Application.JobPosts.Responses;
using TimeSwap.Application.Location.Responses;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Specs;

namespace TimeSwap.Application.Mappings
{
    public class CoreMappingProfile : Profile
    {
        public CoreMappingProfile()
        {
            CreateMap<JobPost, JobPostResponse>().ReverseMap();
            CreateMap<Pagination<JobPost>, Pagination<JobPostResponse>>().ReverseMap();
            CreateMap<Pagination<Category>, Pagination<CategoryResponse>>().ReverseMap();
            CreateMap<City, CityResponse>().ReverseMap();
            CreateMap<Ward, WardResponse>().ReverseMap();
            CreateMap<Industry, IndustryResponse>().ReverseMap();
            CreateMap<Category, CategoryResponse>().ReverseMap();
        }
    }
}
