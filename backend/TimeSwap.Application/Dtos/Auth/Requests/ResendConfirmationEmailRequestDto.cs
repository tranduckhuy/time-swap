namespace TimeSwap.Application.Dtos.Auth.Requests
{
    public class ResendConfirmationEmailRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string ClientUrl { get; set; } = string.Empty;
    }
}
