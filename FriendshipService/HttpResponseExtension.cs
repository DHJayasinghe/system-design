using System.Net;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;

namespace FriendshipService;

public static class HttpResponseExtension
{
    public static HttpResponseData OkResult(this HttpRequestData req, string responseText = "")
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json; charset=utf-8");
        if (responseText.Length != 0)
            response.WriteString(responseText);
        return response;
    }

    public static HttpResponseData BadResult(this HttpRequestData req, string responseText = "")
    {
        var response = req.CreateResponse(HttpStatusCode.BadRequest);
        response.Headers.Add("Content-Type", "application/json; charset=utf-8");
        if (responseText.Length != 0)
            response.WriteString(responseText);
        return response;
    }

    public static HttpResponseData ErrorResult(this HttpRequestData req, string responseText = "")
    {
        var response = req.CreateResponse(HttpStatusCode.InternalServerError);
        response.Headers.Add("Content-Type", "application/json; charset=utf-8");
        if (responseText.Length != 0)
            response.WriteString(responseText);
        return response;
    }

    public static HttpResponseData OkObjectResult<T>(this HttpRequestData req, T responseObject)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json; charset=utf-8");
        response.WriteString(JsonConvert.SerializeObject(responseObject));
        return response;
    }
}