using AutoMapper;
using TimeSwap.Application.Categories.Responses;
using TimeSwap.Application.Industries.Responses;
using TimeSwap.Application.JobApplicants.Responses;
using TimeSwap.Application.JobPosts.Commands;
using TimeSwap.Application.JobPosts.Responses;
using TimeSwap.Application.Location.Responses;
using TimeSwap.Application.Payments.Commands;
using TimeSwap.Application.Payments.Responses;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Specs;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Mappings
{
    public class CoreMappingProfile : Profile
    {
        public CoreMappingProfile()
        {
            CreateMap<JobPost, JobPostResponse>()
                .ForMember(dest => dest.OwnerAvatarUrl, opt => opt.MapFrom(src => src.User.AvatarUrl))
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.User.FullName));

            CreateMap<Pagination<JobPost>, Pagination<JobPostResponse>>();
            CreateMap<City, CityResponse>();
            CreateMap<Ward, WardResponse>();
            CreateMap<Industry, IndustryResponse>();
            CreateMap<Category, CategoryResponse>();

            CreateMap<CreateJobPostCommand, JobPost>();
            CreateMap<UpdateJobPostCommand, JobPost>();
            CreateMap<AssignJobCommand, JobApplicant>();

            CreateMap<Payment, PaymentDetailResponse>()
                  .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.Id))
                  .ForMember(dest => dest.PaymentMethodType,
                      opt => opt.MapFrom(src => src.PaymentMethod != null ? src.PaymentMethod.PaymentMethodType.ToString() : string.Empty))
                  .ForMember(dest => dest.MethodDetailName,
                      opt => opt.MapFrom(src => src.PaymentMethod != null ? src.PaymentMethod.MethodDetailName : string.Empty));

            CreateMap<Pagination<Payment>, Pagination<PaymentDetailResponse>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
                .ReverseMap();


            CreateMap<CreatePaymentCommand, Payment>()
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => PaymentStatus.Pending))
                .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => DateTime.UtcNow.AddMinutes(15)))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<Payment, TransactionLog>()
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.TransactionId))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<JobApplicant, JobApplicantResponse>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserAppliedId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.UserApplied.FullName))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.UserApplied.AvatarUrl))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserApplied.Email));

            CreateMap<Pagination<JobApplicant>, Pagination<JobApplicantResponse>>();

            CreateMap<JobPost, JobPostDetailResponse>()
                .ForMember(dest => dest.OwnerAvatarUrl, opt => opt.MapFrom(src => src.User.AvatarUrl))
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.OwnerEmail, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.OwnerLocation, opt => opt.MapFrom(src => src.User.Ward != null ? src.User.Ward.FullLocation : string.Empty));
        }
    }
}
