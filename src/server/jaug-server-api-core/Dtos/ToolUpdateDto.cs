using FluentValidation;

namespace jaug_server_api_core.Dtos
{
    public class ToolUpdateDto : ToolDto
    {
        public int Id { get; set; }
    }

    public class ToolUpdateDtoValidator : AbstractValidator<ToolUpdateDto>
    {
        public ToolUpdateDtoValidator()
        {
            RuleFor(t => t.Id).NotEmpty();
            RuleFor(t => t.Name).NotEmpty();
        }
    }
}