using IdentityServer4.Configuration;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BnA.IAM.Application.Services;

public sealed class TokenCreationService : DefaultTokenCreationService
{
    public TokenCreationService(
        ISystemClock clock,
        IKeyMaterialService keys,
        IdentityServerOptions options,
        ILogger<DefaultTokenCreationService> logger)
        : base(clock, keys, options, logger)
    {
    }

    protected override async Task<string> CreateJwtAsync(JwtSecurityToken jwt)
    {
        var plaintext = $"{jwt.EncodedHeader}.{jwt.EncodedPayload}";

        using var hasher = CryptoHelper.GetHashAlgorithmForSigningAlgorithm(jwt.SignatureAlgorithm);
        var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(plaintext));

        var rsa = new RSACryptoServiceProvider();
        rsa.ImportFromPem(@"-----BEGIN PRIVATE KEY-----
MIICeAIBADANBgkqhkiG9w0BAQEFAASCAmIwggJeAgEAAoGBAK7DdiJSAdkZoIg9
oSu0YDXnqTUSOO4O9fCttPeuqWeUBnbBx8QzdPn3BstB1kXCrlB0xuGLi8JQ5ng/
S9frN0a1SNQeiG6RctP2uatu0ciMkjiVhPvCty35RCae0IqsnAkZ663mnRULcKXw
Ic0pflOhfuFmAkSPJrcNACekkK9HAgMBAAECgYEAkMZFj+rlswai0RpU8NKtPRqb
NubQmI12Ohp8pw5fMfoTXL/tEGEcT5LPYwQ4UHQVWXtT4jZq4d+I/SZaWxV1JNMn
/M+sAp0Pl5+gq9bQtICrXqa/gbHoaXF+/M5QeN13ORkkXyywjMIazGdjotWGKinp
Di5Ecbyii+3CTJ9d29ECQQDo5kgzCNbjx5L50qn5ppqL1SbcIVsA4n61f3CxAYXp
k8lemxb05DtbO5knYXAYd67HQtCwbXpO3AzVqk3DzzzJAkEAwBkBpPtClwUvpgo1
SUtvjvB6nbum0uZYurq8fvgDSUb6ouCz6QOfuqYorKqucC5rUzlo/PpQ2FwXEW/q
ZuxjjwJBAMGRK26nKRrVc3WJPlZMvuP7Ozn3yw/4L0Gf8sRatLbGarXjhnfw/Ng9
t3PAiw764dugz5vi0aWbRFuNGObmZekCQQCn1LE1HTi+jQ9mN8D6emfLMJtQN+S2
mf2nlwKcw77LRLIxn7RPvpTNE+KxiKXC6StnmN77Nw9vGHYnC+p1Zc5NAkA2/r/j
LSoIiE8rxBPsXQ2I2tjtCCgdoSYEvTcbIza1nwz2tavyr5NYFeCuzurMHDxla7qX
NQ9N8RLqyod0TWRW
-----END PRIVATE KEY-----");

        var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
        rsaFormatter.SetHashAlgorithm("SHA256");
        byte[] dataHash = ComputeHash(hash, "SHA256");

        byte[] signature = rsaFormatter.CreateSignature(dataHash);

        return $"{plaintext}.{Base64UrlTextEncoder.Encode(signature)}";
    }

    static byte[] ComputeHash(byte[] data, string hashAlgorithm)
    {
        using HashAlgorithm hasher = HashAlgorithm.Create(hashAlgorithm);
        return hasher.ComputeHash(data);
    }
}