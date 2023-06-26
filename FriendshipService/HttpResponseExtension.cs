using System.Net;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;

namespace FriendshipService;

public static class HttpResponseExtension
***REMOVED***
    public static HttpResponseData OkResult(this HttpRequestData req, string responseText = "")
***REMOVED***
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "***REMOVED***lication/json; charset=utf-8");
        if (responseText.Length != 0)
            response.WriteString(responseText);
        return response;
    ***REMOVED***

    public static HttpResponseData BadResult(this HttpRequestData req, string responseText = "")
***REMOVED***
        var response = req.CreateResponse(HttpStatusCode.BadRequest);
        response.Headers.Add("Content-Type", "***REMOVED***lication/json; charset=utf-8");
        if (responseText.Length != 0)
            response.WriteString(responseText);
        return response;
    ***REMOVED***

    public static HttpResponseData ErrorResult(this HttpRequestData req, string responseText = "")
***REMOVED***
        var response = req.CreateResponse(HttpStatusCode.InternalServerError);
        response.Headers.Add("Content-Type", "***REMOVED***lication/json; charset=utf-8");
        if (responseText.Length != 0)
            response.WriteString(responseText);
        return response;
    ***REMOVED***

    public static HttpResponseData OkObjectResult<T>(this HttpRequestData req, T responseObject)
***REMOVED***
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "***REMOVED***lication/json; charset=utf-8");
        response.WriteString(JsonConvert.SerializeObject(responseObject));
        return response;
    ***REMOVED***
***REMOVED***