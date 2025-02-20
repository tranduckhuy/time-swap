namespace TimeSwap.Application.Authentication.User
{
    public class AddUserProfileRequestDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }
}
