using AutoMapper;
using TimeSwap.Application.Authentication.Dtos.Requests;
using TimeSwap.Application.Authentication.User;
using TimeSwap.Auth.Models.Requests;

namespace TimeSwap.Auth.Mappings
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            CreateMap<RegisterRequestDto, RegisterRequest>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.Trim()))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName.Trim()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Trim()))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber.Trim()))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password.Trim()))
                .ReverseMap();

            CreateMap<LoginRequestDto, LoginRequest>().ReverseMap();
            CreateMap<ForgotPasswordRequestDto, ForgotPasswordRequest>().ReverseMap();
            CreateMap<ResetPasswordRequestDto, ResetPasswordRequest>().ReverseMap();
            CreateMap<ConfirmEmailRequestDto, ConfirmEmailRequest>().ReverseMap();
            CreateMap<ResendConfirmationEmailRequestDto, ResendConfirmationEmailRequest>().ReverseMap();
            CreateMap<RefreshTokenDto, RefreshTokenRequest>().ReverseMap();

            // User
            CreateMap<UpdateUserProfileRequest, UpdateUserProfileRequestDto>().ReverseMap();
            CreateMap<UpdateSubscriptionRequest, UpdateSubscriptionRequestDto>().ReverseMap();
            CreateMap<ChangePasswordRequest, ChangePasswordRequestDto>().ReverseMap();

            CreateMap<LockUnlockAccountRequest, LockUnlockAccountRequestDto>().ReverseMap();
        }
    }
}
