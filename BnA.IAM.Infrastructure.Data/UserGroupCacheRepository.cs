using BnA.IAM.Application.Common.Interfaces;
using BnA.IAM.Domain.Entities;
using BnA.PM.SharedKernel;
using BnA.PM.SharedKernel.Interfaces;
using BnA.PM.SharedKernel.Specification;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BnA.IAM.Infrastructure.Data;

public sealed class UserGroupCacheRepository : IUserGroupRepository
{
    private readonly IUserGroupRepository _userGroupRepo;
    private readonly IDatabaseMemoryCache _memoryCache;
    private readonly ILogger<UserGroupCacheRepository> _logger;

    public UserGroupCacheRepository(
        IUserGroupRepository accountRepo,
        IDatabaseMemoryCache memoryCache,
        ILogger<UserGroupCacheRepository> logger)
    {
        _userGroupRepo = accountRepo ?? throw new ArgumentNullException(nameof(accountRepo));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Maybe<ApplicationRole>> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
    {
        string key = $"{nameof(ApplicationRole)}{id}";
        if (_memoryCache.HasCache(key, out Maybe<ApplicationRole> entry))
        {
            _logger.LogInformation("Query execution returns user group result from Cache.");
            return entry;
        }

        entry = await _userGroupRepo.GetByIdAsync(id, cancellationToken);
        if (entry.HasValue)
            _memoryCache.SetSmallCacheWithLongLifeSpan(key, entry);

        _logger.LogInformation("Query execution returns user group result from DB.");
        return entry;
    }

    public async Task<List<ApplicationRole>> ListAsync(Specification<ApplicationRole> specification, CancellationToken cancellationToken = default)
    {
        string key = $"{nameof(ApplicationRole)}{specification.GetHashCode()}";
        if (_memoryCache.HasCache(key, out List<ApplicationRole> entires))
        {
            _logger.LogInformation("Query execution returns user group results from Cache.");
            return entires;
        }

        entires = await _userGroupRepo.ListAsync(specification, cancellationToken);
        if (entires != null && entires.Any())
            _memoryCache.SetSmallCacheWithLongLifeSpan(key, entires);

        _logger.LogInformation("Query execution returns user groups result from DB.");
        return entires;
    }

    public async Task<ApplicationRole> SingleAsync(Specification<ApplicationRole> specification, CancellationToken cancellationToken = default)
    {
        string key = $"{nameof(ApplicationRole)}{specification.GetHashCode()}";
        if (_memoryCache.HasCache(key, out ApplicationRole entire))
        {
            _logger.LogInformation("Query execution returns user group results from Cache.");
            return entire;
        }

        entire = await _userGroupRepo.SingleAsync(specification,cancellationToken);
        if (entire != null)
            _memoryCache.SetSmallCacheWithLongLifeSpan(key, entire);

        _logger.LogInformation("Query execution returns user group result from DB.");
        return entire;

    }
      
}
