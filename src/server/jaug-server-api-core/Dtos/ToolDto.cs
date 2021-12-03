using System.Collections.Generic;

namespace jaug_server_api_core.Dtos
{
    public class ToolDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<CommandReadDto> Commands { get; set; }
    }
}
