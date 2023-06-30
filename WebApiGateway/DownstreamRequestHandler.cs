using System.Security.Claims;

namespace WebApiGateway
{

    internal sealed class DownstreamRequestHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly CurrentUser _currentUser;

        public DownstreamRequestHandler(IHttpContextAccessor contextAccessor, CurrentUser currentUser)
        {
            _contextAccessor = contextAccessor;
            _currentUser = currentUser;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //string CorrelationId = GetCorrelationId();

            request.Headers.Add("user-id", _currentUser.Id);

            //do stuff and optionally call the base handler..
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            return response;
        }
    }

    public sealed class CurrentUser 
    {
        private readonly HttpContext _httpContext;
        public CurrentUser(IHttpContextAccessor httpContextAccessor) => _httpContext = httpContextAccessor.HttpContext;
        public CurrentUser(HttpContext httpContext) => _httpContext = httpContext;

        public bool IsAuthenticated => _httpContext.User.Identity.IsAuthenticated;

        private string GetNameIdentifier() => IsAuthenticated ? (_httpContext.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value : null;
        public string Id => GetNameIdentifier() != null ? GetNameIdentifier() : null;
    }
}
