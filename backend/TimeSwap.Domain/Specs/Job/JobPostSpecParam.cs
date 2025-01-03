using TimeSwap.Shared.Constants;

namespace TimeSwap.Domain.Specs.Job
{
    public class JobPostSpecParam : BaseSpecParam
    {
        public int IndustryId { get; set; }
        public int CategoryId { get; set; }
        public string? Sort { get; set; }
        public string? Search { get; set; }
        public decimal? MinFee { get; set; }
        public decimal? MaxFee { get; set; }
        public PostedDate? PostedDate { get; set; }
        public string? CityId { get; set; }
        public string? WardId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
