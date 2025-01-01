namespace TimeSwap.Domain.Specs
{
    public abstract class BaseSpecParam
    {
        public const int MaxPageSize = 70;
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
