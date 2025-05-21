// Middleware/LoggingMiddleware.cs
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace JamilDotnetMicrosoftCertificate.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next   = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation("Incoming: {Method} {Path}",
                context.Request.Method, context.Request.Path);

            await _next(context);

            _logger.LogInformation("Outgoing: {StatusCode}",
                context.Response.StatusCode);
        }
    }
}
