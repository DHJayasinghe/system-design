***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***

***REMOVED***

***REMOVED***
***REMOVED***

***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED*** // cookie policy to deal with temporary browser incompatibilities
***REMOVED***
***REMOVED***.AddHttpClient();


***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***

***REMOVED***

***REMOVED***
    ***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***

***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***;

***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
        googleOptions.ClientId = "1085351065661-1q46g7icejprtfv42ut9gqdo9hs2249a.***REMOVED***s.googleusercontent.com";
        googleOptions.ClientSecret = "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";
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***;

***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***);


builder.Host.UseSerilog((context, ***REMOVED***, configuration) => configuration
***REMOVED***
    .ReadFrom.Services(***REMOVED***)
***REMOVED***
    .WriteTo.ApplicationInsights(***REMOVED***.GetRequiredService<Teleme***REMOVED***Configuration>(), Teleme***REMOVED***Converter.Traces));

***REMOVED***

***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***

***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***;

***REMOVED***
***REMOVED***
***REMOVED***
    .WriteTo.ApplicationInsights(***REMOVED***.Services.GetRequiredService<Teleme***REMOVED***Configuration>(), Teleme***REMOVED***Converter.Traces)
***REMOVED***

string ***REMOVED***lication = ***REMOVED***.Configuration.GetValue<string>("ApplicationName");

***REMOVED***
***REMOVED***
    Log.Information($"***REMOVED******REMOVED***lication***REMOVED*** ***REMOVED***lication starting up");
    ***REMOVED***.Run();
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
    Log.Fatal(ex, $"***REMOVED******REMOVED***lication***REMOVED*** ***REMOVED***lication failed to start");
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
