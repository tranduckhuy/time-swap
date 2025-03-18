namespace TimeSwap.Application.Authentication.Dtos.Requests
{
    public class LockUnlockAccountRequestDto
    {
        public Guid UserId { get; set; }
        public string? Reason { get; set; }
        public bool IsLocked { get; set; }
    }
}
