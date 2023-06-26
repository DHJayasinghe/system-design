using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

***REMOVED***

var environment = builder.Environment.EnvironmentName;


builder.Configuration
    .AddJsonFile(environment == "Development" ? "ocelot-dev.json" : "ocelot.json", optional: false, reloadOnChange: true);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("JwtBearer", options =>
 ***REMOVED***
         options.Authority = builder.Configuration["IdP:Issuer"];
         options.TokenValidationParameters = new TokenValidationParameters
     ***REMOVED***
             ValidateLifetime = false,
             ValidateAudience = false,
             ValidateIssuer = false,
 ***REMOVED***
 ***REMOVED***;
builder.Services
    .AddCors(options =>
***REMOVED***
        options.AddDefaultPolicy(policy => policy
            .WithOrigins(builder.Configuration.GetSection("AllowedCors").Get<string[]>())
            .AllowAnyMethod()
            .AllowAnyHeader()
        );
***REMOVED***;
builder.Services
    .AddOcelot(builder.Configuration);

***REMOVED***

***REMOVED***
    .UseAuthentication()
***REMOVED***;

if (***REMOVED***.Environment.IsDevelopment())
***REMOVED***
    ***REMOVED***.UseDeveloperExceptionPage();
***REMOVED***

***REMOVED***
    .UseCors()
    .UseOcelot()
    .Wait();

***REMOVED***.Run();