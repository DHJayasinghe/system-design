using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using WebApiGateway;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment.EnvironmentName;


builder.Configuration
    .AddJsonFile(environment == "Development" ? "ocelot-dev.json" : "ocelot.json", optional: false, reloadOnChange: true);

builder
    .Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.Authority = builder.Configuration["IdP:Issuer"];
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = false,
                ValidateAudience = false,
                ValidateIssuer = false,
            };
        });
builder
    .Services
       .AddCors(options =>
       {
           options.AddDefaultPolicy(policy => policy
               .WithOrigins(builder.Configuration.GetSection("AllowedCors").Get<string[]>())
               .AllowAnyMethod()
               .AllowAnyHeader()
           );
       })
       .AddOcelot(builder.Configuration);

var app = builder.Build();

app
    .UseAuthentication()
    .UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app
    .UseCors()
    .UseMiddleware<AuthenticateUserIdMiddleware>()
    .UseOcelot()
    .Wait();

app.Run();