using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BnA.IAM.Application.Stores;

public sealed class ValidationKeysStore : IValidationKeysStore
{
    private readonly ISigningCredentialStore _signingCredentialStore;

    public ValidationKeysStore(ISigningCredentialStore signingCredentialStore) =>
        _signingCredentialStore = signingCredentialStore ?? throw new ArgumentNullException(nameof(signingCredentialStore));

    public async Task<IEnumerable<SecurityKeyInfo>> GetValidationKeysAsync()
    {
        var credentials = await _signingCredentialStore.GetSigningCredentialsAsync();
        IEnumerable<SecurityKeyInfo> validationKeys = new List<SecurityKeyInfo>() { new SecurityKeyInfo
        {
            Key = credentials.Key,
            SigningAlgorithm = credentials.Algorithm
        } };
        return validationKeys;
    }
}
