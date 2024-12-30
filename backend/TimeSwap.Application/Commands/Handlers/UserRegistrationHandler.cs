using MediatR;
using TimeSwap.Application.Responses;

namespace TimeSwap.Application.Commands.Handlers
{
    public class UserRegistrationHandler : IRequestHandler<UserRegistrationCommand, AuthenticationResponse>
    {
        public UserRegistrationHandler()
        {
        }

        public Task<AuthenticationResponse> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
