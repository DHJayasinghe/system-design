using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;

namespace FriendshipService;

public static class HttpResponseExtension
{
    public static HttpResponseData OkResult(this HttpRequestData req, string responseText = "")
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/json; charset=utf-8");
        if (responseText.Length != 0)
            response.WriteString(responseText);
        return response;
    }

    public static HttpResponseData BadResult(this HttpRequestData req, string responseText = "")
    {
        var response = req.CreateResponse(HttpStatusCode.BadRequest);
        response.Headers.Add("Content-Type", "text/json; charset=utf-8");
        if (responseText.Length != 0)
            response.WriteString(responseText);
        return response;
    }

    public static HttpResponseData ErrorResult(this HttpRequestData req, string responseText = "")
    {
        var response = req.CreateResponse(HttpStatusCode.InternalServerError);
        response.Headers.Add("Content-Type", "text/json; charset=utf-8");
        if (responseText.Length != 0)
            response.WriteString(responseText);
        return response;
    }

    public static async Task<HttpResponseData> OkObjectResultAsync<T>(this HttpRequestData req, T responseObject)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/json; charset=utf-8");
        await response.WriteAsJsonAsync(responseObject);
        return response;
    }
}