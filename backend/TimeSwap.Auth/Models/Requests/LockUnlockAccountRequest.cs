using System.Text.Json.Serialization;

namespace TimeSwap.Auth.Models.Requests
{
    public class LockUnlockAccountRequest
    {
        [JsonRequired]
        public Guid UserId { get; set; }
        public string? Reason { get; set; }
    }
}
