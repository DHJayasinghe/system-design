using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;

namespace NotificationService;

public static class Function1
{
    [FunctionName("Function1")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
        ILogger log)
    {
        using var smtpClient = new SmtpClient("localhost", 25);
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential("admin", "admin");
        smtpClient.EnableSsl = false;

        // Create and configure the email message
        var message = new MailMessage
        {
            From = new MailAddress("info@facebook.com")
        };
        message.To.Add(new MailAddress("recipient@example.com"));
        message.Subject = "Dhanuka Jayasinghe added a post";
        message.IsBodyHtml = true;
        message.Body = "This is the body of the email...";

        // Send the email
        smtpClient.Send(message);

        return new OkResult();
    }
}
