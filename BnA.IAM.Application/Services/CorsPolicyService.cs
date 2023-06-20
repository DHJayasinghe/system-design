using BnA.IAM.Application.Stores;
using IdentityServer4.Services;
using System.Linq;
using System.Threading.Tasks;

namespace BnA.IAM.Application.Services;

public sealed class CorsPolicyService : ICorsPolicyService
{
    public async Task<bool> IsOriginAllowedAsync(string origin) =>  await Task.FromResult(ClientStore.Clients.Any(d => d.AllowedCorsOrigins.Contains(origin)));
}