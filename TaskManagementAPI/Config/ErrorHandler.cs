using System.Text.Json;
using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Config
{
    public class ErrorHandler
    {
        private readonly RequestDelegate _next;

        public ErrorHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = TaskResponse<object>.Fail(
                    "Ocurrió un error inesperado.",
                    error.Message
                );

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }

    }
}
