using Azure;
using Azure.Data.Tables;
***REMOVED***
using System.Security.Cryptography;
using System.Text;

namespace UserService.Models;

public class User
***REMOVED***
    public string Id ***REMOVED*** get; set; ***REMOVED***
    public string Email ***REMOVED*** get; set; ***REMOVED***
    public string PhoneNumber ***REMOVED*** get; set; ***REMOVED***
    public string FirstName ***REMOVED*** get; set; ***REMOVED***
    public string Surname ***REMOVED*** get; set; ***REMOVED***
    public string Username ***REMOVED*** get; set; ***REMOVED***
    public DateTime? DateOfBirth ***REMOVED*** get; set; ***REMOVED***
    public string Gender ***REMOVED*** get; set; ***REMOVED***
    public DateTime CreatedDateTime ***REMOVED*** get; set; ***REMOVED*** = DateTime.UtcNow;
    internal static string HashData(string data)
***REMOVED***
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
        var base64String = Convert.ToBase64String(hashBytes);

        var alphanumericString = new StringBuilder();
        foreach (var c in base64String)
    ***REMOVED***
            if (!char.IsLetterOrDigit(c)) continue;
            alphanumericString.Append(c);
        ***REMOVED***

        return alphanumericString.ToString();
    ***REMOVED***
***REMOVED***

public class UserEntity : BaseTableEntity
***REMOVED***
    public string Email ***REMOVED*** get; set; ***REMOVED***
    public string PhoneNumber ***REMOVED*** get; set; ***REMOVED***
    public string FirstName ***REMOVED*** get; set; ***REMOVED***
    public string Surname ***REMOVED*** get; set; ***REMOVED***
    public string Username ***REMOVED*** get; set; ***REMOVED***
    public DateTime? DateOfBirth ***REMOVED*** get; set; ***REMOVED***
    public string Gender ***REMOVED*** get; set; ***REMOVED***
    public DateTime CreatedDateTime ***REMOVED*** get; set; ***REMOVED***
    public string NameIdentifier ***REMOVED*** get; internal set; ***REMOVED***
    public string Name ***REMOVED*** get; internal set; ***REMOVED***
***REMOVED***


public static class M***REMOVED***ings
***REMOVED***
    public static UserEntity ToEntity(this User user)
***REMOVED***
        string username = user.Email ?? user.PhoneNumber;
        string id = User.HashData(username);

        return new UserEntity()
    ***REMOVED***
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
***REMOVED***
    ***REMOVED***

    public static UserEntity ToEntity(this RegisterUserAccountRequest user)
***REMOVED***
        string id = User.HashData(user.Email);

        return new UserEntity()
    ***REMOVED***
            PartitionKey = id,
            RowKey = id,
            CreatedDateTime = DateTime.UtcNow,
            NameIdentifier = user.NameIdentifier,
            Name = user.Name,
            FirstName = user.GivenName,
            Surname = user.Surname,
            Email = user.Email,
            Username = user.Email,
***REMOVED***
    ***REMOVED***

    public static User ToUser(this UserEntity todo)
***REMOVED***
        return new User()
    ***REMOVED***
            Id = todo.PartitionKey,
            CreatedDateTime = todo.CreatedDateTime,
            FirstName = todo.FirstName,
            Surname = todo.Surname,
            Gender = todo.Gender,
            DateOfBirth = todo.DateOfBirth,
            Email = todo.Email,
            PhoneNumber = todo.PhoneNumber,
            Username = todo.Username,
***REMOVED***
    ***REMOVED***

***REMOVED***

public class BaseTableEntity : ITableEntity
***REMOVED***
    public string PartitionKey ***REMOVED*** get; set; ***REMOVED***
    public string RowKey ***REMOVED*** get; set; ***REMOVED***
    public DateTimeOffset? Timestamp ***REMOVED*** get; set; ***REMOVED***
    public ETag ETag ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
