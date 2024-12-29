namespace TimeSwap.Application.Interfaces.Services
{
    public interface ITokenBlackListService
    {
        Task BlacklistTokenAsync(string token, DateTime expiry);
        Task<bool> IsTokenBlacklistedAsync(string token);
    }
}
