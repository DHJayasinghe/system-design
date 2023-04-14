***REMOVED***
using Microsoft.Azure.WebJobs;
***REMOVED***

namespace UrlShortenerService;

public class KeepAppWarmUpFunction
***REMOVED***
    [FunctionName(nameof(KeepAppWarmUpFunction))]
    public static void Run([TimerTrigger("0 */15 * * * *")] TimerInfo myTimer, ILogger log)
    ***REMOVED***
        log.LogInformation("***REMOVED***0***REMOVED*** function executed at: ***REMOVED***1***REMOVED***", nameof(KeepAppWarmUpFunction), DateTime.UtcNow);
***REMOVED***
***REMOVED***
