using System.ComponentModel.DataAnnotations;

namespace jaug_server_api_core.Data.Entities
{
    public class Command : BaseEntity
    {
        [Required]
        [MaxLength(100)] // e.g. nvarchar(100) - for more significant app would have to consider tradeoff of mixing concerns vs perf/efficiency with something like - [Column(TypeName = "varchar(100)")] 
        public string Description { get; set; }
        [Required]
        [MaxLength(200)] 
        public string Syntax { get; set; }
        [MaxLength(200)]
        public string Example { get; set; }

        [Required]
        public int ToolId { get; set; }
        public Tool Tool { get; set; }
    }
}
