using MediatR;
using TimeSwap.Application.Responses;

namespace TimeSwap.Application.Commands
{
    public record UserRegistrationCommand(string Email, string Password) : IRequest<AuthenticationResponse>;
}
