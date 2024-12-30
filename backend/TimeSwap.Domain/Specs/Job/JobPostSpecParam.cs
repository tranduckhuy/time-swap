namespace TimeSwap.Domain.Specs.Job
{
    public class JobPostSpecParam
    {
        public const int MaxPageSize = 70;
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
        public int IndustryId { get; set; }
        public int CategoryId { get; set; }
        public string? Sort { get; set; }
        public string? Search { get; set; }
        public decimal? MinFee { get; set; }
        public decimal? MaxFee { get; set; }
        public DateTime? PostedDate { get; set; }
        public List<string> LocationIds { get; set; } = [];
    }
}
