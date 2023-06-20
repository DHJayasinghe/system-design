using BnA.IAM.Application.Common.Interfaces.Services;
using IdentityServer4.Configuration;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace BnA.IAM.Application.Services;

public sealed class TokenCreationService : DefaultTokenCreationService
{
    private readonly IKeyVaultService _keyVaultService;

    public TokenCreationService(
        ISystemClock clock,
        IKeyMaterialService keys,
        IdentityServerOptions options,
        IKeyVaultService keyVaultService,
        ILogger<DefaultTokenCreationService> logger)
        : base(clock, keys, options, logger)
    {
        _keyVaultService = keyVaultService ?? throw new ArgumentNullException(nameof(keyVaultService));
    }

    protected override async Task<string> CreateJwtAsync(JwtSecurityToken jwt)
    {
        var plaintext = $"{jwt.EncodedHeader}.{jwt.EncodedPayload}";

        using var hasher = CryptoHelper.GetHashAlgorithmForSigningAlgorithm(jwt.SignatureAlgorithm);
        var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(plaintext));

        var signResult = await _keyVaultService.SignAsync("idp-token-signing-ec-key", jwt.SignatureAlgorithm, hash);

        return $"{plaintext}.{Base64UrlTextEncoder.Encode(signResult.Signature)}";
    }
}