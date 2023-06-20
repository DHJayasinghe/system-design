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

public sealed class AccountCacheRepository : IAccountRepository
{
    private readonly IAccountRepository _accountRepo;
    private readonly IDatabaseMemoryCache _memoryCache;
    private readonly ILogger<AccountCacheRepository> _logger;

    public AccountCacheRepository(
        IAccountRepository accountRepo,
        IDatabaseMemoryCache memoryCache,
        ILogger<AccountCacheRepository> logger)
    {
        _accountRepo = accountRepo ?? throw new ArgumentNullException(nameof(accountRepo));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Add(ApplicationUser entity) => _accountRepo.Add(entity);

    public void Remove(ApplicationUser entity) => _accountRepo.Remove(entity);
    public async Task<Maybe<UserAccount>> FindByExternalRef(string externalOrganizationRef = null, string externalUserRef = null, Guid? userId = null, Guid? organizationId = null)
    {
        string key = $"{nameof(ApplicationUser)}{externalUserRef}{externalOrganizationRef}{userId}{organizationId}";
        if (_memoryCache.HasCache(key, out Maybe<UserAccount> entry))
        {
            _logger.LogInformation("Query execution returns account username + organizationId result from Cache.");
            return entry;
        }

        entry = await _accountRepo.FindByExternalRef(externalUserRef: externalUserRef, externalOrganizationRef: externalOrganizationRef, userId: userId, organizationId: organizationId);
        if (entry.HasValue) _memoryCache.SetSmallCacheWithLongLifeSpan(key, entry);

        _logger.LogInformation("Query execution returns account username + organizationId result from DB.");
        return entry;
    }

    public async Task<Maybe<ApplicationUser>> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull =>
        await _accountRepo.GetByIdAsync(id, cancellationToken);

    public async Task<Maybe<ApplicationUser>> GetByExternalIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
    {
        string key = $"{nameof(ApplicationUser)}ByExternalId{id}";
        if (_memoryCache.HasCache(key, out Maybe<ApplicationUser> entry))
        {
            _logger.LogInformation("Query execution returns account by external ref result from Cache.");
            return entry;
        }

        entry = await _accountRepo.GetByExternalIdAsync(id, cancellationToken);
        if (entry.HasValue) _memoryCache.SetSmallCacheWithLongLifeSpan(key, entry);

        _logger.LogInformation("Query execution returns account by external ref result from DB.");
        return entry;
    }


    public async Task<List<ApplicationUser>> ListAsync(Specification<ApplicationUser> specification, CancellationToken cancellationToken = default)
    {
        string key = $"{nameof(ApplicationUser)}{specification.GetHashCode()}";
        if (_memoryCache.HasCache(key, out List<ApplicationUser> entires))
        {
            _logger.LogInformation("Query execution returns accounts result from Cache.");
            return entires;
        }

        entires = await _accountRepo.ListAsync(specification, cancellationToken);
        if (entires != null && entires.Any())
            _memoryCache.SetMediumCacheWithShortLifeSpan(key, entires);

        _logger.LogInformation("Query execution returns accounts result from DB.");
        return entires;
    }

    public async Task<ApplicationUser> SingleAsync(Specification<ApplicationUser> specification, CancellationToken cancellationToken = default) =>
       await _accountRepo.SingleAsync(specification, cancellationToken);
    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _accountRepo.SaveChangesAsync(cancellationToken);
}
