using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UserService.Models;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Azure;

namespace UserService;

public static class PostUserFunction
{
    [FunctionName(nameof(PostUserFunction))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "user")] HttpRequest req,
        [Table("user")] IAsyncCollector<UserEntity> profile,
        [Table("user")] TableClient existingProfiles,
        ILogger log)
    {
        string requestBody = new StreamReader(req.Body).ReadToEnd();
        dynamic data = JsonConvert.DeserializeObject(requestBody);

        log.LogInformation("{0} function processed a request", nameof(PostUserFunction));

        var user = new User()
        {
            FirstName = data?.firstName,
            Surname = data?.surname,
            Gender = data?.gender,
            Email = data?.email,
            PhoneNumber = data?.phoneNumber
        };
        var entitiy = user.ToEntity();

        if (RecordExist(existingProfiles, entitiy))
            return new BadRequestObjectResult($"User account already exist with username: {entitiy.Username}");

        await profile.AddAsync(entitiy);

        return new OkObjectResult(entitiy.PartitionKey);
    }

    private static bool RecordExist(TableClient existingProfiles, UserEntity entitiy)
    {
        try
        {
            _ = existingProfiles.GetEntity<UserEntity>(entitiy.PartitionKey, entitiy.PartitionKey);
            return true;
        }
        catch (RequestFailedException ex)
        {
            if (ex.ErrorCode == "ResourceNotFound") return false;
            return true;
        }
    }
}
