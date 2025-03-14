namespace TimeSwap.Application.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message message);
        Task SendEmailBrevoAsync(string receiverEmail, string receiverName, string subject, string message);
    }
}
