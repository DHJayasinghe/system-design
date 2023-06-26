using Azure;
using Azure.Data.Tables;
using BnA.IAM.Application.Exceptions;
***REMOVED***
using Microsoft.Extensions.Logging;
***REMOVED***
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BnA.IAM.Application.Services;

public sealed class TableStorageService : ITableStorageService
***REMOVED***
    private readonly ILogger<TableStorageService> _logger;
    private readonly string _accountName;
    private readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=simadfutilityfuncaue;AccountKey=S3a3RBEnzDqPv/IWYy2njQB5TXIOnvnQbSLw1WK4oaM3c8qG4cfA8g+ub5glW2qClVMGT5izM5tz+ASt3MqyyQ==;EndpointSuffix=core.windows.net";

    public TableStorageService(ILogger<TableStorageService> logger)
***REMOVED***
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    ***REMOVED***

    public async Task<List<TResult>> ReadAsync<TResult>(string partitionKey, string tableName) where TResult : class, new()
***REMOVED***
        string tableEndpoint = string.Format("https://***REMOVED***0***REMOVED***.table.core.windows.net", _accountName);
        _logger.LogInformation($"Retrieving data from storage table: ***REMOVED***tableEndpoint***REMOVED***/***REMOVED***tableName***REMOVED*** - partitionKey: ***REMOVED***partitionKey***REMOVED***");

        string _partitionKey = partitionKey.ClearKey();
        string filter = TableClient.CreateQueryFilter($"PartitionKey eq ***REMOVED***_partitionKey***REMOVED***");

        ***REMOVED***
    ***REMOVED***
            var tableClient = new TableClient(_connectionString, tableName);
            tableClient.CreateIfNotExists();
            Pageable<TableEntity> entities = tableClient.Query<TableEntity>(filter: filter);

            return await Task.FromResult(entities
                .AsEnumerable()
                .Select(entity => entity.ToDictionary(d => d.Key, d => d.Value)
                .ToObject<TResult>())
                .ToList());
        ***REMOVED***
        ***REMOVED***
    ***REMOVED***
            throw new TableStorageException("Unhandled exception during storage table entity query", ex);
        ***REMOVED***
    ***REMOVED***

    public async Task<int> WriteAsync<TPayload>(string partitionKey, string tableName, TPayload payload, string rowKey = null) where TPayload : class, new()
***REMOVED***
        string tableEndpoint = string.Format("https://***REMOVED***0***REMOVED***.table.core.windows.net", _accountName);
        _logger.LogInformation($"Writing table entity to storage table: ***REMOVED***tableEndpoint***REMOVED***/***REMOVED***tableName***REMOVED*** - partitionKey: ***REMOVED***partitionKey***REMOVED***");

        string _partitionKey = partitionKey.ClearKey();
        var tableEntity = new TableEntity(payload.ToDictionary())
    ***REMOVED***
            PartitionKey = _partitionKey,
            RowKey = rowKey ?? _partitionKey
***REMOVED***

        ***REMOVED***
    ***REMOVED***
            var tableClient = new TableClient(_connectionString, tableName);
            tableClient.CreateIfNotExists();
            var response = await tableClient.UpsertEntityAsync(tableEntity, TableUpdateMode.Replace);

            return response.Status;
        ***REMOVED***
        ***REMOVED***
    ***REMOVED***
            throw new TableStorageException("Unhandled exception occured during storage table entity upsert", ex);
        ***REMOVED***
    ***REMOVED***

    public async Task<int> RemoveAsync(string partitionKey, string tableName)
***REMOVED***
        string tableEndpoint = string.Format("https://***REMOVED***0***REMOVED***.table.core.windows.net", _accountName);
        _logger.LogInformation($"Removing table entity from storage table: ***REMOVED***tableEndpoint***REMOVED***/***REMOVED***tableName***REMOVED*** - partitionKey: ***REMOVED***partitionKey***REMOVED***");

        ***REMOVED***
    ***REMOVED***
            string _partitionKey = partitionKey.ClearKey();
            var tableClient = new TableClient(_connectionString, tableName);
            var resonse = await tableClient.DeleteEntityAsync(partitionKey.ClearKey(), _partitionKey);

            return resonse.Status;
        ***REMOVED***
        ***REMOVED***
    ***REMOVED***
            throw new TableStorageException("Unhandled exception occured during storage table entity delete", ex);
        ***REMOVED***
    ***REMOVED***
***REMOVED***
