using System;

namespace BnA.IAM.Application.Exceptions;

public sealed class TableStorageException : Exception
{
    public TableStorageException(string message)
        : base(message) { }

    public TableStorageException(string message, Exception innerException)
        : base(message, innerException) { }
}
