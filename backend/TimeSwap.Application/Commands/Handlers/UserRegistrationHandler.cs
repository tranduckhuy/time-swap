using MediatR;
using TimeSwap.Application.Responses;
using TimeSwap.Domain.Repositories;

namespace TimeSwap.Application.Commands.Handlers
{
    public class UserRegistrationHandler : IRequestHandler<UserRegistrationCommand, AuthenticationResponse>
    {
        private readonly IAuthRepository _authRepository;

        public UserRegistrationHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public Task<AuthenticationResponse> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
