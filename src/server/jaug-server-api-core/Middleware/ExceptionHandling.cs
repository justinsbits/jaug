using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace jaug_server_api_core.Middleware
{
    // Erik Dahl / PluralSight - docker-dot-net-core-apps-developing
    public class ExceptionHandling
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionHandling(RequestDelegate next, ILogger<ExceptionHandling> logger)
        {
            _next = next;
            _logger = logger;
        }

        // middleware method to support minimal need for try/catch in controllers - address handling exceptions here
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleGlobalExceptionAsync(context, ex);
            }
        }

        // central point for catching unexpected server side (500) exceptions
        // validations should not result in exception - using BadRequest return
        private Task HandleGlobalExceptionAsync(HttpContext context, Exception exception)
        {
            var errorId = Guid.NewGuid();
                _logger.LogError(exception, "An error occurred: {ErrorId}", errorId);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return context.Response.WriteAsJsonAsync(new
                {
                    ErrorId = errorId,
                    // <update> as needed with some user friendly/facing message here, keeping in mind security etc. in terms of details provided
                    Message = "An error occurred within the API." 
                });
            //}
        }
    }
}
