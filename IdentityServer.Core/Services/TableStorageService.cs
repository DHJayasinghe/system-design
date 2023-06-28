using Azure;
using Azure.Data.Tables;
using BnA.IAM.Application.Exceptions;
using BnA.IAM.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BnA.IAM.Application.Services;

public sealed class TableStorageService : ITableStorageService
{
    private readonly ILogger<TableStorageService> _logger;
    private readonly string _connectionString;

    public TableStorageService(ILogger<TableStorageService> logger, IConfiguration configurations)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _connectionString = configurations.GetConnectionString("StorageAccount");
    }

    public async Task<List<TResult>> ReadAsync<TResult>(string partitionKey, string tableName) where TResult : class, new()
    {
        _logger.LogInformation($"Retrieving data from storage table: {tableName} - partitionKey: {partitionKey}");

        string _partitionKey = partitionKey.ClearKey();
        string filter = TableClient.CreateQueryFilter($"PartitionKey eq {_partitionKey}");

        try
        {
            var tableClient = new TableClient(_connectionString, tableName);
            tableClient.CreateIfNotExists();
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
        _logger.LogInformation($"Writing table entity to storage table: {tableName} - partitionKey: {partitionKey}");

        string _partitionKey = partitionKey.ClearKey();
        var tableEntity = new TableEntity(payload.ToDictionary())
        {
            PartitionKey = _partitionKey,
            RowKey = rowKey ?? _partitionKey
        };

        try
        {
            var tableClient = new TableClient(_connectionString, tableName);
            tableClient.CreateIfNotExists();
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
        _logger.LogInformation($"Removing table entity from storage table: {tableName} - partitionKey: {partitionKey}");

        try
        {
            string _partitionKey = partitionKey.ClearKey();
            var tableClient = new TableClient(_connectionString, tableName);
            var resonse = await tableClient.DeleteEntityAsync(partitionKey.ClearKey(), _partitionKey);

            return resonse.Status;
        }
        catch (Exception ex)
        {
            throw new TableStorageException("Unhandled exception occured during storage table entity delete", ex);
        }
    }
}
