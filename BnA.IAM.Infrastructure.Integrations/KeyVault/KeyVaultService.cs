using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using BnA.IAM.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BnA.IAM.Infrastructure.Integrations.KeyVault;

public sealed class KeyVaultService : IKeyVaultService
{
    private readonly ILogger<KeyVaultService> _logger;
    private readonly ChainedTokenCredential _credentials;
    private readonly string _keyVaultUri;

    public KeyVaultService(
        KeyVaultConfig config,
        ChainedTokenCredential credentials,
        ILogger<KeyVaultService> logger)
    {
        _credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _keyVaultUri = config.Uri;
    }

    public async Task<KeyVaultKey> GetKeyAsync(string key)
    {
        _logger.LogInformation($"Retrieving key from key vault: {key}");

        var keyClient = new KeyClient(new Uri(_keyVaultUri), _credentials);
        Response<KeyVaultKey> response = await keyClient.GetKeyAsync(key);
        return response.Value;
    }

    public async Task<SignResult> SignAsync(string key, string algorithm, byte[] hash)
    {
        _logger.LogInformation($"Signing hash using key vault: {key}");

        var cryptoClient = new CryptographyClient(new Uri($"{_keyVaultUri}/keys/{key}"), _credentials);
        return await cryptoClient.SignAsync(new SignatureAlgorithm(algorithm), hash);
    }
}

public sealed class KeyVaultConfig
{
    public string Uri { get; set; }
}