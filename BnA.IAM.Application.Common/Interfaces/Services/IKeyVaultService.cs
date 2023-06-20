using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using System.Threading.Tasks;

namespace BnA.IAM.Application.Common.Interfaces.Services;

public interface IKeyVaultService
{
    Task<KeyVaultKey> GetKeyAsync(string key);

    Task<SignResult> SignAsync(string key, string algorithm, byte[] hash);
}