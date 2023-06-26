﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BnA.IAM.Presentation.API.Controllers;

public class SecurityHeadersAttribute : ActionFilterAttribute
***REMOVED***
    public override void OnResultExecuting(ResultExecutingContext context)
***REMOVED***
        var result = context.Result;
        if (result is ViewResult)
    ***REMOVED***
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Content-Type-Options
            if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Type-Options"))
        ***REMOVED***
                context.HttpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            ***REMOVED***

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options
            //if (!context.HttpContext.Response.Headers.ContainsKey("X-Frame-Options"))
            //***REMOVED***
            //    //context.HttpContext.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            //***REMOVED***

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
            var csp = "default-src 'self'; object-src 'none';";
            csp += "frame-ancestors 'self';";
            csp += "sandbox allow-forms allow-same-origin allow-scripts; base-uri 'self';";
            csp += "font-src 'self' fonts.googleapis.com fonts.gstatic.com;";
            csp += "style-src-elem 'self' fonts.googleapis.com;";
            csp += "upgrade-insecure-requests;";
            // also an example if you need client images to be displayed from twitter
            // csp += "img-src 'self' https://pbs.twimg.com;";

            // once for standards compliant browsers
            if (!context.HttpContext.Response.Headers.ContainsKey("Content-Security-Policy"))
        ***REMOVED***
                context.HttpContext.Response.Headers.Add("Content-Security-Policy", csp);
            ***REMOVED***
            // and once again for IE
            if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Security-Policy"))
        ***REMOVED***
                context.HttpContext.Response.Headers.Add("X-Content-Security-Policy", csp);
            ***REMOVED***

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy
            var referrer_policy = "no-referrer";
            if (!context.HttpContext.Response.Headers.ContainsKey("Referrer-Policy"))
        ***REMOVED***
                context.HttpContext.Response.Headers.Add("Referrer-Policy", referrer_policy);
            ***REMOVED***
        ***REMOVED***
    ***REMOVED***
***REMOVED***
