***REMOVED***
using BnA.IAM.Presentation.API.Controllers.Account;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace BnA.IAM.Presentation.API.Controllers
***REMOVED***
    public static class Extensions
***REMOVED***
        /// <summary>
        /// Checks if the redirect URI is for a native client.
        /// </summary>
        /// <returns></returns>
        public static bool IsNativeClient(this AuthorizationRequest context)
    ***REMOVED***
            return !context.RedirectUri.StartsWith("https", StringComparison.Ordinal)
               && !context.RedirectUri.StartsWith("http", StringComparison.Ordinal);
        ***REMOVED***

        public static IActionResult LoadingPage(this Controller controller, string viewName, string redirectUri)
    ***REMOVED***
            controller.HttpContext.Response.StatusCode = 200;
            controller.HttpContext.Response.Headers["Location"] = "";

            return controller.View(viewName, new RedirectViewModel ***REMOVED*** RedirectUrl = redirectUri ***REMOVED***);
        ***REMOVED***
    ***REMOVED***
***REMOVED***
