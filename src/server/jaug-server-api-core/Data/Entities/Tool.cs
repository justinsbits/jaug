using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace jaug_server_api_core.Data.Entities
{
    public class Tool : BaseEntity
    {
        [Required]
        [MaxLength(100)] // e.g. nvarchar(100) - for more significant app would have to consider tradeoff of mixing concerns vs perf/efficiency with something like - [Column(TypeName = "varchar(100)")] 
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public ICollection<Command> Commands { get; set; } = new List<Command>();
    }
}
