using BnA.IAM.Application.Common.Interfaces;
using BnA.PM.SharedKernel.Helpers;
using BnA.PM.SharedKernel.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BnA.IAM.Infrastructure.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserGroupRepository, UserGroupRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();

        return services;
    }

    public static IServiceCollection AddCacheRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDatabaseMemoryCache>(new DatabaseMemoryCache(configuration.GetSection(nameof(MemoryCacheConfig)).Get<MemoryCacheConfig>()));
        services.Decorate<IUserGroupRepository, UserGroupCacheRepository>();
        services.Decorate<IAccountRepository, AccountCacheRepository>();

        return services;
    }
}
