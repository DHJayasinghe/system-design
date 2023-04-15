using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UserService.Models;
using System.Threading.Tasks;

namespace UserService;

public static class PostUserFunction
{
    [FunctionName(nameof(PostUserFunction))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "user")] HttpRequest req,
        [Table("user", Connection = "AzureWebJobsStorage")] IAsyncCollector<UserEntity> profile,
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
        var entitiy = user.ToTableEntity();


        await profile.AddAsync(entitiy);

        return new OkObjectResult(entitiy.PartitionKey);
    }
}
