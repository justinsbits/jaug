
namespace jaug_server_api_core.Controllers
{
    public class ToolsResourceParameters
    {
        const int maxPageSize = 20;
        public bool IncludeCommands { get; set; } = false;

        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }

        public string OrderBy { get; set; } = "Name";
        public string Fields { get; set; }
    }
}
