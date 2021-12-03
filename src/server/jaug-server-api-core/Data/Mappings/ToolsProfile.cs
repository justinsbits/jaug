using AutoMapper;
using jaug_server_api_core.Dtos;
using jaug_server_api_core.Data.Entities;

namespace jaug_server_api_core.Data.Mappings
{
    public class ToolsProfile : Profile
    {
        public ToolsProfile()
        {
            //src -> dest
            CreateMap<Tool, ToolReadDto>();
            CreateMap<ToolCreateDto, Tool>();
            CreateMap<ToolUpdateDto, Tool>().ReverseMap(); // support both src->dest and dest->src
            CreateMap<Command, CommandReadDto>();
        }
    }
}
