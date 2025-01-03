using AutoMapper;
using TimeSwap.Application.Categories.Responses;
using TimeSwap.Application.Industries.Responses;
using TimeSwap.Application.JobApplicants.Responses;
using TimeSwap.Application.JobPosts.Commands;
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

            CreateMap<CreateJobPostCommand, JobPost>();
            CreateMap<UpdateJobPostCommand, JobPost>();
            CreateMap<AssignJobCommand, JobApplicant>();
            CreateMap<JobApplicant, JobApplicantResponse>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserAppliedId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.UserApplied.FullName))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.UserApplied.AvatarUrl))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserApplied.Email));
        }
    }
}
