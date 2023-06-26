using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
***REMOVED***
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BnA.IAM.Application.Stores;

public sealed class ResourceStore : IResourceStore
***REMOVED***
    private List<ApiResource> apiResources = new()
***REMOVED***
        new ApiResource
    ***REMOVED***
            Name = "local.api",
            DisplayName = "Local API",
            Description = "Local API",
            Enabled = true,
            UserClaims = new[]***REMOVED***
                "id",
                "email"
            ***REMOVED***,
            Scopes = new[] ***REMOVED*** "https://local.api" ***REMOVED***
        ***REMOVED***
    ***REMOVED***;
    public static readonly IEnumerable<IdentityResource> IdentityResources = new IdentityResource[]
***REMOVED***
        // some standard scopes from the OIDC spec
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Email()
    ***REMOVED***;

    public async Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
***REMOVED***
        return (await GetApiResourcesAsync())
            .Where(resource => apiResourceNames.Contains(resource.Name));
    ***REMOVED***

    public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
***REMOVED***
        return (await GetApiResourcesAsync())
            .Where(resource => resource.Scopes != null
                && resource.Scopes.Any()
                && scopeNames.Any(d => resource.Scopes.Contains(d)));
    ***REMOVED***

    public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
***REMOVED***
        return (await GetApiScopesAsync()).Where(scope => scopeNames.Contains(scope.Name));
    ***REMOVED***

    public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames) =>
        Task.FromResult(IdentityResources.Where(resource => scopeNames.Contains(resource.Name)));

    public async Task<Resources> GetAllResourcesAsync()
***REMOVED***
        var resources = GetApiResourcesAsync();
        var scopes = GetApiScopesAsync();
        await Task.WhenAll(resources, scopes);
        return new Resources
    ***REMOVED***
            ApiResources = resources.Result.ToList(),
            ApiScopes = scopes.Result.ToList(),
            IdentityResources = IdentityResources.ToList(),
***REMOVED***
    ***REMOVED***

    private async Task<IEnumerable<ApiResource>> GetApiResourcesAsync() => await Task.FromResult(apiResources);

    public async Task<IEnumerable<ApiScope>> GetApiScopesAsync()
***REMOVED***
        var apiScopes = (await GetApiResourcesAsync())
            .Where(d => d.Enabled).SelectMany(d => d.Scopes)
            .Select(scope => new ApiScope(scope))
            .ToList();
        apiScopes.Add(new ApiScope("IdentityServerApi"));
        return apiScopes;
    ***REMOVED***
***REMOVED***