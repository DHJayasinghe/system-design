#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebApiGateway/WebApiGateway.csproj", "WebApiGateway/"]
RUN dotnet restore "WebApiGateway/WebApiGateway.csproj"
COPY . .
WORKDIR "/src/WebApiGateway"
RUN dotnet build "WebApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApiGateway.dll"]