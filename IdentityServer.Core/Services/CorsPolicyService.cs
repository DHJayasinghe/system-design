***REMOVED***
using Duende.IdentityServer.Services;
using System.Linq;
using System.Threading.Tasks;

namespace BnA.IAM.Application.Services;

public sealed class CorsPolicyService : ICorsPolicyService
***REMOVED***
    public async Task<bool> IsOriginAllowedAsync(string origin) =>  await Task.FromResult(ClientStore.Clients.Any(d => d.AllowedCorsOrigins.Contains(origin)));
***REMOVED***