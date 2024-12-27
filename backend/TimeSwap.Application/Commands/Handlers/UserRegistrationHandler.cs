using MediatR;
using TimeSwap.Application.Interfaces.Services;
using TimeSwap.Application.Responses;

namespace TimeSwap.Application.Commands.Handlers
{
    public class UserRegistrationHandler : IRequestHandler<UserRegistrationCommand, AuthenticationResponse>
    {
        private readonly IAuthService _authService;

        public UserRegistrationHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<AuthenticationResponse> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
