using BnA.IAM.Application;
using BnA.IAM.Application.Services;
using BnA.IAM.Application.Stores;
using BnA.IAM.Presentation.API.Extensions;
using Duende.IdentityServer;
using Duende.IdentityServer.Configuration;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;

var builder = WebApplication.CreateBuilder(args);

var configurations = builder.Configuration;
var services = builder.Services;

services
    .AddApplicationInsightsTelemetry(configurations)
     .AddControllersWithViews();
services
    .AddApplication(configurations);
services // cookie policy to deal with temporary browser incompatibilities
    .AddSameSiteCookiePolicy();
services.AddHttpClient();


services
    .AddIdentityServer(options =>
    {
        options.Events.RaiseSuccessEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = false;

        options.EmitScopesAsSpaceDelimitedStringInJwt = true;

        options.Caching = new CachingOptions
        {
            ClientStoreExpiration = TimeSpan.FromHours(2),
            CorsExpiration = TimeSpan.FromHours(1),
            ResourceStoreExpiration = TimeSpan.FromHours(2),
        };
    })
    .AddInMemoryCaching()
    .AddJwtBearerClientAuthentication()
    .AddAppAuthRedirectUriValidator()
    .AddCorsPolicyService<CorsPolicyService>()
    .AddCorsPolicyCache<CorsPolicyService>()
    .AddClientStoreCache<ClientStore>()
    .AddProfileService<UserProfileService>()
    .AddResourceStoreCache<ResourceStore>()
    .AddPersistedGrantStore<PersistedGrantStore>();

services
    .AddSession(op =>
    {
        op.IdleTimeout = TimeSpan.FromSeconds(60);
        op.Cookie.SameSite = SameSiteMode.None;
        op.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    });

services
    .AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        googleOptions.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
        googleOptions.ClientId = "1085351065661-1q46g7icejprtfv42ut9gqdo9hs2249a.apps.googleusercontent.com";
        googleOptions.ClientSecret = "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";
        googleOptions.Scope.Clear();
        googleOptions.Scope.Add("openid");
        googleOptions.Scope.Add("https://www.googleapis.com/auth/userinfo.email");
        googleOptions.Scope.Add("https://www.googleapis.com/auth/userinfo.profile");
    });

services
    .AddLocalApiAuthentication();
services
    .AddCors(options => options.AddPolicy(name: "AllowAnonymousPolicy", builder =>
    {
        builder
            .WithOrigins(configurations.GetSection("AllowedCors").Get<string[]>())
            .WithMethods("get", "post")
            .AllowAnyHeader();
    }));


builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .WriteTo.Console()
    .WriteTo.ApplicationInsights(services.GetRequiredService<TelemetryConfiguration>(), TelemetryConverter.Traces));

var app = builder.Build();

app
    .UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    })
    .UseCertificateForwarding()
    .UseCookiePolicy()
    .UseSerilogRequestLogging()
    //.UseDeveloperExceptionPage()
    .UseStaticFiles();

app
    .UseRouting()
    .UseSession()
    .UseIdentityServer()
    .UseAuthorization()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapDefaultControllerRoute();
    });

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configurations)
    .WriteTo.Console()
    .WriteTo.ApplicationInsights(app.Services.GetRequiredService<TelemetryConfiguration>(), TelemetryConverter.Traces)
    .CreateLogger();

string application = app.Configuration.GetValue<string>("ApplicationName");

try
{
    Log.Information($"{application} application starting up");
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, $"{application} application failed to start");
    return 1;
}
finally
{
    // close and dispose the logging system correctly
    Log.CloseAndFlush();
}
