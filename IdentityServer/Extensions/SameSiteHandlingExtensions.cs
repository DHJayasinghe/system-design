***REMOVED***
***REMOVED***
***REMOVED***

namespace BnA.IAM.Presentation.API.Extensions;

// copied from https://devblogs.microsoft.com/aspnet/upcoming-samesite-cookie-changes-in-asp-net-and-asp-net-core/
public static class SameSiteHandlingExtensions
***REMOVED***
    public static IServiceCollection AddSameSiteCookiePolicy(this IServiceCollection ***REMOVED***)
***REMOVED***
        ***REMOVED***.Configure<CookiePolicyOptions>(options =>
    ***REMOVED***
            options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
            options.OnAppendCookie = cookieContext =>
                CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            options.OnDeleteCookie = cookieContext =>
                CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
    ***REMOVED***;

        return ***REMOVED***;
    ***REMOVED***

    private static void CheckSameSite(HttpContext httpContext, CookieOptions options)
***REMOVED***
        if (options.SameSite == SameSiteMode.None)
    ***REMOVED***
            var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
            if (!httpContext.Request.IsHttps || DisallowsSameSiteNone(userAgent))
        ***REMOVED***
                // For .NET Core < 3.1 set SameSite = (SameSiteMode)(-1)
                options.SameSite = SameSiteMode.Unspecified;
            ***REMOVED***
        ***REMOVED***
    ***REMOVED***

    private static bool DisallowsSameSiteNone(string userAgent)
***REMOVED***
        // Cover all iOS based browsers here. This includes:
        // - Safari on iOS 12 for iPhone, iPod Touch, iPad
        // - WkWebview on iOS 12 for iPhone, iPod Touch, iPad
        // - Chrome on iOS 12 for iPhone, iPod Touch, iPad
        // All of which are broken by SameSite=None, because they use the iOS networking stack
        if (userAgent.Contains("CPU iPhone OS 12") || userAgent.Contains("iPad; CPU OS 12"))
    ***REMOVED***
            return true;
        ***REMOVED***

        // Cover Mac OS X based browsers that use the Mac OS networking stack. This includes:
        // - Safari on Mac OS X.
        // This does not include:
        // - Chrome on Mac OS X
        // Because they do not use the Mac OS networking stack.
        if (userAgent.Contains("Macintosh; Intel Mac OS X 10_14") &&
            userAgent.Contains("Version/") && userAgent.Contains("Safari"))
    ***REMOVED***
            return true;
        ***REMOVED***

        // Cover Chrome 50-69, because some versions are broken by SameSite=None, 
        // and none in this range require it.
        // Note: this covers some pre-Chromium Edge versions, 
        // but pre-Chromium Edge does not require SameSite=None.
        if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
    ***REMOVED***
            return true;
        ***REMOVED***

        return false;
    ***REMOVED***
***REMOVED***
