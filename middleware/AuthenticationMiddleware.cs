// Middleware/AuthenticationMiddleware.cs
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace JamilDotnetMicrosoftCertificate.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthenticationMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("Authorization", out var header) ||
                !header.ToString().StartsWith("Bearer "))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }
            await _next(context);
        }
    }
}
