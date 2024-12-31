namespace TimeSwap.Application.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message message);
    }
}
