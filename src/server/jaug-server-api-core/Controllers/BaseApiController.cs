using Microsoft.AspNetCore.Mvc;

namespace jaug_server_api_core.Controllers
{
    /// <summary>
    /// Abstract BaseApi Controller Class
    /// </summary>
    [ApiController] // among other things, auto returns 400 (BadRequest) on validation errors based on model attribution - where not relying on another validation framework (e.g. FluentValidation)
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BaseApiController : ControllerBase
    {

    }
}
