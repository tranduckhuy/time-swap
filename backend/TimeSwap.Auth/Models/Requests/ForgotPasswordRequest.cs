using System.ComponentModel.DataAnnotations;

namespace TimeSwap.Auth.Models.Requests
{
    public class ForgotPasswordRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Client URI is required")]
        public string ClientUrl { get; set; } = string.Empty;
    }
}
