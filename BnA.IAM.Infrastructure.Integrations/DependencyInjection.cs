using Azure.Identity;
using BnA.IAM.Application.Common.Interfaces.Services;
using BnA.IAM.Infrastructure.Integrations.KeyVault;
using BnA.IAM.Infrastructure.Integrations.TableStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BnA.IAM.Infrastructure.Integrations;

public static class DependencyInjection
{
    public static IServiceCollection AddIntegrations(this IServiceCollection services, IConfiguration configuration)
    {
        var credentials = Guid.Parse(configuration["ManagedIdentityClientId"]) != default
            ? new ChainedTokenCredential(new ManagedIdentityCredential(configuration["ManagedIdentityClientId"]))
            : new ChainedTokenCredential(new VisualStudioCredential(), new AzureCliCredential());

        services
            .AddSingleton(credentials)
            .AddSingleton(new KeyVaultConfig { Uri = configuration["KeyVaultUri"] })
            .AddScoped<ITableStorageService, TableStorageService>()
            .AddScoped<IKeyVaultService, KeyVaultService>();

        return services;
    }
}