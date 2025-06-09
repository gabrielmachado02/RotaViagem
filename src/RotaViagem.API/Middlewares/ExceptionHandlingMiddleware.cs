using FluentValidation;
using System.Net;
using System.Text.Json;

namespace RotaViagem.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

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
            _logger.LogError(exception, "An unhandled exception has occurred: {Message}", exception.Message);

            var statusCode = HttpStatusCode.InternalServerError;
            var response = new
            {
                title = "Server Error",
                status = (int)statusCode,
                detail = "An internal server error has occurred.",
                errors = new List<string>()
            };

            if (exception is ValidationException validationException)
            {
                statusCode = HttpStatusCode.BadRequest;
                var errors = validationException.Errors.Select(e => e.ErrorMessage).ToList();
                response = new
                {
                    title = "Validation Error",
                    status = (int)statusCode,
                    detail = "One or more validation errors occurred.",
                    errors
                };
            }
            else if (exception is KeyNotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                response = new
                {
                    title = "Resource Not Found",
                    status = (int)statusCode,
                    detail = exception.Message,
                    errors = new List<string>()
                };
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}