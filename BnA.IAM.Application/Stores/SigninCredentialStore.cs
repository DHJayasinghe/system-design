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
    public async Task<SigningCredentials> GetSigningCredentialsAsync()
    {
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        rsa.ImportFromPem(@"-----BEGIN PUBLIC KEY-----
MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCuw3YiUgHZGaCIPaErtGA156k1
EjjuDvXwrbT3rqlnlAZ2wcfEM3T59wbLQdZFwq5QdMbhi4vCUOZ4P0vX6zdGtUjU
HohukXLT9rmrbtHIjJI4lYT7wrct+UQmntCKrJwJGeut5p0VC3Cl8CHNKX5ToX7h
ZgJEjya3DQAnpJCvRwIDAQAB
-----END PUBLIC KEY-----
");
        RsaSecurityKey rsaSecurityKey = new RsaSecurityKey(rsa);
        return new SigningCredentials(rsaSecurityKey, "PS256");
    }
}
