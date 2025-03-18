using TimeSwap.Domain.Entities;

namespace TimeSwap.Application.Visistor
{
    public interface IVisitorService
    {
        Task IncreaseVisitCount();
        Task<List<MonthlyVisit>> GetVisitsInRange(int startYear, int startMonth, int endYear, int endMonth);
    }
}
