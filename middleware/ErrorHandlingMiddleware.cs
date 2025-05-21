// Middleware/ErrorHandlingMiddleware.cs
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace JamilDotnetMicrosoftCertificate.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next   = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                context.Response.StatusCode        = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType       = "application/json";
                var payload                        = new { error = "Internal server error." };
                await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
            }
        }
    }
}
