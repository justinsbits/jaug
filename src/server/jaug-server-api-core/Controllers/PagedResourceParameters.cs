
namespace jaug_server_api_core.Controllers
{
    public class PagedResourceParameters
    {
        private const int DEFAULT_MAX_PAGE_SIZE = 20;
        private const int DEFAULT_PAGE_SIZE = 10;
        private const int DEFAULT_PAGE_NUMBER = 1;

        private readonly int _maxPageSize;

        public PagedResourceParameters()
        {
            _maxPageSize = DEFAULT_MAX_PAGE_SIZE;
        }

        public int PageNumber { get; set; } = DEFAULT_PAGE_NUMBER;
        private int _pageSize = DEFAULT_PAGE_SIZE;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
        }

        public string OrderBy { get; set; }
        public string Fields { get; set; }
    }
}
