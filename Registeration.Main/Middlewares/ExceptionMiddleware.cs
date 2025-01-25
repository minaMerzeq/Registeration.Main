
using Registeration.Main.Application.Helpers;
using System.Net;
using System.Text.Json;

namespace Registeration.Main.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            _logger.LogError($"{exception.Message}\n inner exception: {exception.InnerException?.Message}");
            
            var res = JsonSerializer.Serialize(new Response<object>
            {
                Status = false,
                Message = "Internal Server Error",
            });
            await context.Response.WriteAsync(res);
        }
    }
}
