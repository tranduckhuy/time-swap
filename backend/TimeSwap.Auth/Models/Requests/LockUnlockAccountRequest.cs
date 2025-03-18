using System.ComponentModel.DataAnnotations;

namespace TimeSwap.Auth.Models.Requests
{
    public class LockUnlockAccountRequest
    {
        [Required]
        public Guid UserId { get; set; }
        public string? Reason { get; set; }
    }
}
