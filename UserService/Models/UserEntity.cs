using Azure;
using Azure.Data.Tables;
using System;
using System.Security.Cryptography;
using System.Text;

namespace UserService.Models;

public class User
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Gender { get; set; }
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
    internal static string HashData(string data)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
        var base64String = Convert.ToBase64String(hashBytes);

        var alphanumericString = new StringBuilder();
        foreach (var c in base64String)
        {
            if (!char.IsLetterOrDigit(c)) continue;
            alphanumericString.Append(c);
        }

        return alphanumericString.ToString();
    }
}

public class UserEntity : BaseTableEntity
{
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Gender { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public string NameIdentifier { get; internal set; }
    public string Name { get; internal set; }
}


public static class Mappings
{
    public static UserEntity ToEntity(this User user)
    {
        string username = user.Email ?? user.PhoneNumber;
        string id = User.HashData(username);

        return new UserEntity()
        {
            PartitionKey = id,
            RowKey = id,
            CreatedDateTime = user.CreatedDateTime,
            FirstName = user.FirstName,
            Surname = user.Surname,
            Gender = user.Gender,
            DateOfBirth = user.DateOfBirth,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Username = username,
        };
    }

    public static UserEntity ToEntity(this RegisterUserAccountRequest user)
    {
        string id = User.HashData(user.Email);

        return new UserEntity()
        {
            PartitionKey = id,
            RowKey = id,
            CreatedDateTime = DateTime.UtcNow,
            NameIdentifier = user.NameIdentifier,
            Name = user.Name,
            FirstName = user.GivenName,
            Surname = user.Surname,
            Email = user.Email,
            Username = user.Email,
        };
    }

    public static User ToUser(this UserEntity todo)
    {
        return new User()
        {
            Id = todo.PartitionKey,
            CreatedDateTime = todo.CreatedDateTime,
            FirstName = todo.FirstName,
            Surname = todo.Surname,
            Gender = todo.Gender,
            DateOfBirth = todo.DateOfBirth,
            Email = todo.Email,
            PhoneNumber = todo.PhoneNumber,
            Username = todo.Username,
        };
    }

}

public class BaseTableEntity : ITableEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
