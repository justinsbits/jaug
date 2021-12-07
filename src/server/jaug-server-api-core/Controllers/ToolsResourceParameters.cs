
namespace jaug_server_api_core.Controllers
{
    public class ToolsResourceParameters : PagedResourceParameters
    {
        public bool IncludeCommands { get; set; } = false;
    }
}
