using Microsoft.EntityFrameworkCore;
using TimeSwap.Application.Visistor;
using TimeSwap.Domain.Entities;
using TimeSwap.Infrastructure.Persistence.DbContexts;

namespace TimeSwap.Infrastructure.Visistor
{
    public class VisitorService : IVisitorService
    {
        private readonly UserIdentityDbContext _dbContext;

        public VisitorService(UserIdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task IncreaseVisitCount()
        {
            int currentYear = DateTime.UtcNow.Year;
            int currentMonth = DateTime.UtcNow.Month;

            var visitRecord = await _dbContext.MonthlyVisits
                .FirstOrDefaultAsync(v => v.Year == currentYear && v.Month == currentMonth);

            if (visitRecord == null)
            {
                visitRecord = new MonthlyVisit
                {
                    Year = currentYear,
                    Month = currentMonth,
                    VisitCount = 1
                };
                _dbContext.MonthlyVisits.Add(visitRecord);
            }
            else
            {
                visitRecord.VisitCount++;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<MonthlyVisit>> GetVisitsInRange(int startYear, int startMonth, int endYear, int endMonth)
        {
            return await _dbContext.MonthlyVisits
                .Where(v =>
                    (v.Year > startYear || (v.Year == startYear && v.Month >= startMonth)) &&
                    (v.Year < endYear || (v.Year == endYear && v.Month <= endMonth)))
                .OrderBy(v => v.Year)
                .ThenBy(v => v.Month)
                .ToListAsync();
        }
    }

}
