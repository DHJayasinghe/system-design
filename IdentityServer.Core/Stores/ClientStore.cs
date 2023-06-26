using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Duende.IdentityServer;

namespace BnA.IAM.Application.Stores;

public sealed class ClientStore : IClientStore
{

    public static List<Client> Clients = new()
    {
        new Client()
        {
            ClientId = "144e251b-30ff-4027-be96-0623e40cbc19",
            ClientName = "Angular SPA",
            RequireConsent = false,
            RequireClientSecret = false,
            RequirePkce = true,
            AllowedGrantTypes = new[] { "authorization_code" },
            AllowOfflineAccess = true,
            AllowedScopes = new List<string> { },
            RefreshTokenUsage = TokenUsage.OneTimeOnly,
            AbsoluteRefreshTokenLifetime = 86400,
            SlidingRefreshTokenLifetime = 14400,
            AccessTokenLifetime = 1800,
            AlwaysIncludeUserClaimsInIdToken = true,
            RedirectUris = new[] { "http://localhost:4200/sign-in" },
            AllowedCorsOrigins = new[] { "http://localhost:4200" },
            FrontChannelLogoutUri = "http://localhost:4200/sign-out",
            PostLogoutRedirectUris = new[] { "http://localhost:4200/sign-out" },
            UpdateAccessTokenClaimsOnRefresh = true,
            Enabled = true,
            EnableLocalLogin = false
        }
    };
    static readonly string[] allowedDefaultScopes =
    {
        IdentityServerConstants.StandardScopes.OpenId,
        IdentityServerConstants.StandardScopes.Profile,
        IdentityServerConstants.StandardScopes.Email,
        IdentityServerConstants.StandardScopes.OfflineAccess,
        IdentityServerConstants.LocalApi.ScopeName
    };

    public async Task<Client> FindClientByIdAsync(string clientId)
    {
        var client = Clients.FirstOrDefault();

        if (client != null)
            allowedDefaultScopes.ToList().ForEach(scope => client.AllowedScopes.Add(scope));
        client.RequirePkce = false;
        client.RequireClientSecret = false;
        return client;
    }

    public async Task<IEnumerable<Client>> GetClientsAsync()
    {
        return Clients;
    }
}