# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 9002

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["backend/TimeSwap.Api/TimeSwap.Api.csproj", "backend/TimeSwap.Api/"]
COPY ["backend/TimeSwap.Application/TimeSwap.Application.csproj", "backend/TimeSwap.Application/"]
COPY ["backend/TimeSwap.Infrastructure/TimeSwap.Infrastructure.csproj", "backend/TimeSwap.Infrastructure/"]
COPY ["backend/TimeSwap.Domain/TimeSwap.Domain.csproj", "backend/TimeSwap.Domain/"]
COPY ["backend/TimeSwap.Shared/TimeSwap.Shared.csproj", "backend/TimeSwap.Shared/"]
RUN dotnet restore "./backend/TimeSwap.Api/TimeSwap.Api.csproj"
COPY . .
WORKDIR "/src/backend/TimeSwap.Api"
RUN dotnet build "./TimeSwap.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TimeSwap.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TimeSwap.Api.dll"]