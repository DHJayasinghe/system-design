using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace SharedKernal;

public record class CurrentUser : ICurrentUser
{
    private readonly HttpContext _httpContext;
    public CurrentUser(IHttpContextAccessor httpContextAccessor) => _httpContext = httpContextAccessor.HttpContext;

    private string GetNameIdentifier()
    {
        _httpContext.Request.Headers.TryGetValue("user-id", out StringValues userId);
        return userId.First();
    }
    public string Id => GetNameIdentifier();

}