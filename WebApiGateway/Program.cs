using Ocelot.DependencyInjection;
using Ocelot.Middleware;

***REMOVED***

var environment = builder.Environment.EnvironmentName;

builder.Services
    .AddOcelot(builder.Configuration);
builder.Configuration
    .AddJsonFile(environment == "Development" ? "ocelot-dev.json" : "ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddCors(options =>
***REMOVED***
    options.AddDefaultPolicy(policy => policy
        .WithOrigins(builder.Configuration.GetSection("AllowedCors").Get<string[]>())
        .AllowAnyMethod()
        .AllowAnyHeader()
    );
***REMOVED***);

***REMOVED***

if (***REMOVED***.Environment.IsDevelopment())
***REMOVED***
    ***REMOVED***.UseDeveloperExceptionPage();
***REMOVED***

***REMOVED***
    .UseCors()
    .UseOcelot()
    .Wait();

***REMOVED***.Run();