FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /***REMOVED***
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebApiGateway/WebApiGateway.csproj", "WebApiGateway/"]
RUN dotnet restore "WebApiGateway/WebApiGateway.csproj"
COPY . .

FROM build AS publish
RUN dotnet publish "WebApiGateway/WebApiGateway.csproj" -c Release -o /***REMOVED***/publish

FROM base AS final
WORKDIR /***REMOVED***
COPY --from=publish /***REMOVED***/publish .
ENTRYPOINT ["dotnet", "WebApiGateway.dll"]