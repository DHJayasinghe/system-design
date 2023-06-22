using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment.EnvironmentName;

builder.Services
    .AddOcelot(builder.Configuration);
builder.Configuration
    .AddJsonFile(environment == "Development" ? "ocelot-dev.json" : "ocelot.json", optional: false, reloadOnChange: true);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdP:Issuer"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateAudience = false,
            ValidateIssuer = false
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