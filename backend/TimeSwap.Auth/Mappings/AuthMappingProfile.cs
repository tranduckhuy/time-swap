using AutoMapper;
using TimeSwap.Application.Authentication.Dtos.Requests;
using TimeSwap.Auth.Models.Requests;

namespace TimeSwap.Auth.Mappings
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            CreateMap<RegisterRequestDto, RegisterRequest>().ReverseMap();
            CreateMap<LoginRequestDto, LoginRequest>().ReverseMap();
            CreateMap<ForgotPasswordRequestDto, ForgotPasswordRequest>().ReverseMap();
            CreateMap<ResetPasswordRequestDto, ResetPasswordRequest>().ReverseMap();
            CreateMap<ConfirmEmailRequestDto, ConfirmEmailRequest>().ReverseMap();
            CreateMap<ResendConfirmationEmailRequestDto, ResendConfirmationEmailRequest>().ReverseMap();
            CreateMap<RefreshTokenDto, RefreshTokenRequest>().ReverseMap();

        }
    }
}
