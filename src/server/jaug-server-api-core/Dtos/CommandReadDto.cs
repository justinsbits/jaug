using System.ComponentModel.DataAnnotations;

namespace jaug_server_api_core.Dtos
{
    public class CommandReadDto
    {
        public int Id { get; set; }

        public string Description { get; set;}

        public string Syntax {get; set;}
    }
}