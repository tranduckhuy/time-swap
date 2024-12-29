using TimeSwap.Application.Dtos.Email;

namespace TimeSwap.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(Message message);
    }
}
