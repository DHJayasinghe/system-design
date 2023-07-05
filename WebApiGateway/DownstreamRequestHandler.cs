using System.Security.Claims;

namespace WebApiGateway
{
    public sealed class AuthenticateUserIdMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticateUserIdMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext httpContext)
        {
            var userId = (httpContext.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            httpContext.Request.Headers.Add("user-id", userId);
            await _next(httpContext);
        }
    }
}
