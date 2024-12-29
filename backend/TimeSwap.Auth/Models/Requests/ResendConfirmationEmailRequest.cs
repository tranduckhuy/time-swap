using System.ComponentModel.DataAnnotations;

namespace TimeSwap.Auth.Models.Requests
{
    public class ResendConfirmationEmailRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Client URI is required")]
        public string ClientUri { get; set; } = string.Empty;
    }
}
