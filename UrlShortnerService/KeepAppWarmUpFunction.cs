using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace UrlShortenerService;

public class KeepAppWarmUpFunction
{
    [FunctionName(nameof(KeepAppWarmUpFunction))]
    public static void Run([TimerTrigger("0 */15 * * * *",UseMonitor = false)] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation("{0} function executed at: {1}", nameof(KeepAppWarmUpFunction), DateTime.UtcNow);
    }
}
