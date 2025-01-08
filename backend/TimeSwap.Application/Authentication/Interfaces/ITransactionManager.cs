namespace TimeSwap.Application.Authentication.Interfaces
{
    public interface ITransactionManager
    {
        Task ExecuteAsync(Func<Task> action);
    }
}
