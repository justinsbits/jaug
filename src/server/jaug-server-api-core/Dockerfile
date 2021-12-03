#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/server/jaug-server-api-core/jaug-server-api-core.csproj", "src/server/jaug-server-api-core/"]
RUN dotnet restore "src/server/jaug-server-api-core/jaug-server-api-core.csproj"
COPY . .
WORKDIR "/src/src/server/jaug-server-api-core"
RUN dotnet build "jaug-server-api-core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "jaug-server-api-core.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "jaug-server-api-core.dll"]