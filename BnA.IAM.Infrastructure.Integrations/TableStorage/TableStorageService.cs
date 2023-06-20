using Azure;
using Azure.Data.Tables;
using Azure.Identity;
using BnA.IAM.Application.Common.Exceptions;
using BnA.IAM.Application.Common.Interfaces.Services;
using BnA.IAM.Infrastructure.Integrations.TableStorage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BnA.IAM.Infrastructure.Integrations.TableStorage;

public sealed class TableStorageService : ITableStorageService
{
    private readonly ILogger<TableStorageService> _logger;
    private readonly ChainedTokenCredential _credentials;
    private readonly string _accountName;

    public TableStorageService(
        ChainedTokenCredential credentials,
        ILogger<TableStorageService> logger)
    {
        //_accountName = (storageAccountsConfig ?? throw new ArgumentNullException(nameof(storageAccountsConfig))).IdpStorage;
        _credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<TResult>> ReadAsync<TResult>(string partitionKey, string tableName) where TResult : class, new()
    {
        string tableEndpoint = string.Format("https://{0}.table.core.windows.net", _accountName);
        _logger.LogInformation($"Retrieving data from storage table: {tableEndpoint}/{tableName} - partitionKey: {partitionKey}");

        string _partitionKey = partitionKey.ClearKey();
        string filter = TableClient.CreateQueryFilter($"PartitionKey eq {_partitionKey}");

        try
        {
            var tableClient = new TableClient(new Uri(tableEndpoint), tableName, _credentials);
            Pageable<TableEntity> entities = tableClient.Query<TableEntity>(filter: filter);

            return await Task.FromResult(entities
                .AsEnumerable()
                .Select(entity => entity.ToDictionary(d => d.Key, d => d.Value)
                .ToObject<TResult>())
                .ToList());
        }
        catch (Exception ex)
        {
            throw new TableStorageException("Unhandled exception during storage table entity query", ex);
        }
    }

    public async Task<int> WriteAsync<TPayload>(string partitionKey, string tableName, TPayload payload, string rowKey = null) where TPayload : class, new()
    {
        string tableEndpoint = string.Format("https://{0}.table.core.windows.net", _accountName);
        _logger.LogInformation($"Writing table entity to storage table: {tableEndpoint}/{tableName} - partitionKey: {partitionKey}");

        string _partitionKey = partitionKey.ClearKey();
        var tableEntity = new TableEntity(payload.ToDictionary())
        {
            PartitionKey = _partitionKey,
            RowKey = rowKey ?? _partitionKey
        };

        try
        {
            var tableClient = new TableClient(new Uri(tableEndpoint), tableName, _credentials);
            var response = await tableClient.UpsertEntityAsync(tableEntity, TableUpdateMode.Replace);

            return response.Status;
        }
        catch (Exception ex)
        {
            throw new TableStorageException("Unhandled exception occured during storage table entity upsert", ex);
        }
    }

    public async Task<int> RemoveAsync(string partitionKey, string tableName)
    {
        string tableEndpoint = string.Format("https://{0}.table.core.windows.net", _accountName);
        _logger.LogInformation($"Removing table entity from storage table: {tableEndpoint}/{tableName} - partitionKey: {partitionKey}");

        try
        {
            string _partitionKey = partitionKey.ClearKey();
            var tableClient = new TableClient(new Uri(tableEndpoint), tableName, _credentials);
            var resonse = await tableClient.DeleteEntityAsync(partitionKey.ClearKey(), _partitionKey);

            return resonse.Status;
        }
        catch (Exception ex)
        {
            throw new TableStorageException("Unhandled exception occured during storage table entity delete", ex);
        }
    }
}
