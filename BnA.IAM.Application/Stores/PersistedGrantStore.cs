using BnA.IAM.Application.Services;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BnA.IAM.Application.Stores;

public sealed class PersistedGrantStore : IPersistedGrantStore
{
    private const string TABLE_STORE = "IAMGrantStore";
    private readonly ITableStorageService _tableStorageService;

    public PersistedGrantStore(ITableStorageService tableStorageService) =>
        _tableStorageService = tableStorageService ?? throw new ArgumentNullException(nameof(tableStorageService));

    public async Task<IEnumerable<PersistedGrant>> GetAllAsync(PersistedGrantFilter filter) =>
        await _tableStorageService.ReadAsync<PersistedGrant>(filter.SessionId, TABLE_STORE);

    public async Task<PersistedGrant> GetAsync(string key) =>
        (await _tableStorageService.ReadAsync<PersistedGrant>(key, TABLE_STORE)).SingleOrDefault();

    public async Task RemoveAllAsync(PersistedGrantFilter filter)
        => await _tableStorageService.RemoveAsync(filter.SessionId, TABLE_STORE);

    public async Task RemoveAsync(string key) =>
        await _tableStorageService.RemoveAsync(key, TABLE_STORE);

    public async Task StoreAsync(PersistedGrant grant) =>
        await _tableStorageService.WriteAsync(grant.Key, TABLE_STORE, grant);
}