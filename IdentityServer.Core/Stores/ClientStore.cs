using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
***REMOVED***

namespace BnA.IAM.Application.Stores;

public sealed class ClientStore : IClientStore
***REMOVED***

    public static List<Client> Clients = new()
***REMOVED***
        new Client()
    ***REMOVED***
            ClientId = "144e251b-30ff-4027-be96-0623e40cbc19",
            ClientName = "Angular SPA",
            RequireConsent = false,
            RequireClientSecret = false,
            RequirePkce = true,
            AllowedGrantTypes = new[] ***REMOVED*** "authorization_code" ***REMOVED***,
            AllowOfflineAccess = true,
            AllowedScopes = new List<string> ***REMOVED*** ***REMOVED***,
            RefreshTokenUsage = TokenUsage.OneTimeOnly,
            AbsoluteRefreshTokenLifetime = 86400,
            SlidingRefreshTokenLifetime = 14400,
            AccessTokenLifetime = 1800,
            AlwaysIncludeUserClaimsInIdToken = true,
            RedirectUris = new[] ***REMOVED*** "http://localhost:4200/sign-in" ***REMOVED***,
            AllowedCorsOrigins = new[] ***REMOVED*** "http://localhost:4200" ***REMOVED***,
            FrontChannelLogoutUri = "http://localhost:4200/sign-out",
            PostLogoutRedirectUris = new[] ***REMOVED*** "http://localhost:4200/sign-out" ***REMOVED***,
            UpdateAccessTokenClaimsOnRefresh = true,
            Enabled = true,
            EnableLocalLogin = false
        ***REMOVED***
    ***REMOVED***;
    static readonly string[] allowedDefaultScopes =
***REMOVED***
        IdentityServerConstants.StandardScopes.OpenId,
        IdentityServerConstants.StandardScopes.Profile,
        IdentityServerConstants.StandardScopes.Email,
        IdentityServerConstants.StandardScopes.OfflineAccess,
        IdentityServerConstants.LocalApi.ScopeName
    ***REMOVED***;

    public async Task<Client> FindClientByIdAsync(string clientId)
***REMOVED***
        var client = Clients.FirstOrDefault();

        if (client != null)
            allowedDefaultScopes.ToList().ForEach(scope => client.AllowedScopes.Add(scope));
        client.RequirePkce = false;
        client.RequireClientSecret = false;
        return client;
    ***REMOVED***

    public async Task<IEnumerable<Client>> GetClientsAsync()
***REMOVED***
        return Clients;
    ***REMOVED***
***REMOVED***