FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GridSystem.Api/GridSystem.Api.csproj", "GridSystem.Api/"]
COPY ["AppDefinition/AppDefinition.csproj", "AppDefinition/"]
COPY ["EntityFramework.Preferences/EntityFramework.Preferences.csproj", "EntityFramework.Preferences/"]
COPY ["EntityFramework.Configurations/EntityFramework.Configurations.csproj", "EntityFramework.Configurations/"]
COPY ["GridSystem.Domain/GridSystem.Domain.csproj", "GridSystem.Domain/"]
COPY ["EntityFramework.SourceGenerator/EntityFramework.SourceGenerator.csproj", "EntityFramework.SourceGenerator/"]
COPY ["EFSelector/EFSelector.csproj", "EFSelector/"]
COPY ["Sieve/Sieve.csproj", "Sieve/"]
RUN dotnet restore "GridSystem.Api/GridSystem.Api.csproj"
COPY . .
WORKDIR "/src/GridSystem.Api"
RUN dotnet build "GridSystem.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "GridSystem.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GridSystem.Api.dll"]
