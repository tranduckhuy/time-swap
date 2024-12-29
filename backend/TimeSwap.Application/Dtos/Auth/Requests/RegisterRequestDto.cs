namespace TimeSwap.Application.Dtos.Auth.Requests
{
    public class RegisterRequestDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; 
        public string ConfirmPassword { get; set; } = string.Empty;
        public string ClientUrl { get; set; } = string.Empty;
    }
}
