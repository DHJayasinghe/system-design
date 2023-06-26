using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment.EnvironmentName;


builder.Configuration
    .AddJsonFile(environment == "Development" ? "ocelot-dev.json" : "ocelot.json", optional: false, reloadOnChange: true);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("JwtBearer", options =>
     {
         options.Authority = builder.Configuration["IdP:Issuer"];
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateLifetime = false,
             ValidateAudience = false,
             ValidateIssuer = false,
         };
     });
builder.Services
    .AddCors(options =>
    {
        options.AddDefaultPolicy(policy => policy
            .WithOrigins(builder.Configuration.GetSection("AllowedCors").Get<string[]>())
            .AllowAnyMethod()
            .AllowAnyHeader()
        );
    });
builder.Services
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
    .UseOcelot()
    .Wait();

app.Run();