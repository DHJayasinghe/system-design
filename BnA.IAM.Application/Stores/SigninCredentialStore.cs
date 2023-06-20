using Azure.Security.KeyVault.Keys;
using BnA.IAM.Application.Common.Interfaces.Services;
using IdentityServer4.Stores;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BnA.IAM.Application.Stores;

public sealed class SigninCredentialStore : ISigningCredentialStore
{
    private readonly IKeyVaultService _keyVaultService;

    public SigninCredentialStore(IKeyVaultService keyVaultService) =>
        _keyVaultService = keyVaultService ?? throw new ArgumentNullException(nameof(keyVaultService));

    public async Task<SigningCredentials> GetSigningCredentialsAsync()
    {
        var response = await _keyVaultService.GetKeyAsync("idp-token-signing-ec-key");

        AsymmetricSecurityKey key;
        string algorithm;

        if (response.KeyType == KeyType.Ec)
        {
            ECDsa ecDsa = response.Key.ToECDsa();
            key = new ECDsaSecurityKey(ecDsa) { KeyId = response.Properties.Version };

            // parse from curve
            if (response.Key.CurveName == KeyCurveName.P256) algorithm = "ES256";
            else if (response.Key.CurveName == KeyCurveName.P384) algorithm = "ES384";
            else if (response.Key.CurveName == KeyCurveName.P521) algorithm = "ES521";
            else throw new NotSupportedException();
        }
        else if (response.KeyType == KeyType.Rsa)
        {
            RSA rsa = response.Key.ToRSA();
            key = new RsaSecurityKey(rsa) { KeyId = response.Properties.Version };

            // you define
            algorithm = "PS256";
        }
        else
        {
            throw new NotSupportedException();
        }
        return new SigningCredentials(key, algorithm);
    }
}
