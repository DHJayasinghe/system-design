using BnA.IAM.Application.Services;
using BnA.IAM.Application.Stores;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BnA.IAM.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services
           .AddScoped<ITableStorageService, TableStorageService>();
        services
            .AddScoped<ISigningCredentialStore, SigninCredentialStore>()
            .AddScoped<IValidationKeysStore, ValidationKeysStore>()
            .AddTransient<ITokenCreationService, TokenCreationService>();

        return services;
    }
}