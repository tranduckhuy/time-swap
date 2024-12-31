namespace TimeSwap.Application.Authentication.Interfaces
{
    public interface ITokenBlackListService
    {
        Task BlacklistTokenAsync(string token, DateTime expiry);
        Task<bool> IsTokenBlacklistedAsync(string token);
    }
}
