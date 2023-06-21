using System.Collections.Generic;
using System.Threading.Tasks;

namespace BnA.IAM.Application.Services;

public interface ITableStorageService
{
    Task<List<TResult>> ReadAsync<TResult>(string partitionKey, string tableName) where TResult : class, new();

    Task<int> WriteAsync<TPayload>(string partitionKey, string tableName, TPayload payload, string rowKey = null) where TPayload : class, new();

    Task<int> RemoveAsync(string partitionKey, string tableName);
}
