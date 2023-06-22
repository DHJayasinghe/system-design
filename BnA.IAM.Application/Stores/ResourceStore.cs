using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BnA.IAM.Application.Stores;

public sealed class ResourceStore : IResourceStore
{
    private List<ApiResource> apiResources = new()
    {
        new ApiResource
        {
            Name = "local.api",
            DisplayName = "Local API",
            Description = "Local API",
            Enabled = true,
            UserClaims = new[]{
                "id",
                "email"
            },
            Scopes = new[] { "https://local.api" }
        }
    };
    public static readonly IEnumerable<IdentityResource> IdentityResources = new IdentityResource[]
    {
        // some standard scopes from the OIDC spec
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Email()
    };

    public async Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
    {
        return (await GetApiResourcesAsync())
            .Where(resource => apiResourceNames.Contains(resource.Name));
    }

    public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
    {
        return (await GetApiResourcesAsync())
            .Where(resource => resource.Scopes != null
                && resource.Scopes.Any()
                && scopeNames.Any(d => resource.Scopes.Contains(d)));
    }

    public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
    {
        return (await GetApiScopesAsync()).Where(scope => scopeNames.Contains(scope.Name));
    }

    public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames) =>
        Task.FromResult(IdentityResources.Where(resource => scopeNames.Contains(resource.Name)));

    public async Task<Resources> GetAllResourcesAsync()
    {
        var resources = GetApiResourcesAsync();
        var scopes = GetApiScopesAsync();
        await Task.WhenAll(resources, scopes);
        return new Resources
        {
            ApiResources = resources.Result.ToList(),
            ApiScopes = scopes.Result.ToList(),
            IdentityResources = IdentityResources.ToList(),
        };
    }

    private async Task<IEnumerable<ApiResource>> GetApiResourcesAsync() => await Task.FromResult(apiResources);

    public async Task<IEnumerable<ApiScope>> GetApiScopesAsync()
    {
        var apiScopes = (await GetApiResourcesAsync())
            .Where(d => d.Enabled).SelectMany(d => d.Scopes)
            .Select(scope => new ApiScope(scope))
            .ToList();
        apiScopes.Add(new ApiScope("IdentityServerApi"));
        return apiScopes;
    }
}