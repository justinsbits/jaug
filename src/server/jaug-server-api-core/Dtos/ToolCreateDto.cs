using FluentValidation;

namespace jaug_server_api_core.Dtos
{
    public class ToolCreateDto : ToolDto
    {

    }

    public class ToolCreateDtoValidator : AbstractValidator<ToolCreateDto>
    {
        public ToolCreateDtoValidator()
        {
            RuleFor(t => t.Name).NotEmpty();
        }
    }
}
