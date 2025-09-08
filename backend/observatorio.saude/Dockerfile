# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["observatorio.saude/observatorio.saude.csproj", "observatorio.saude/"]
RUN dotnet restore "./observatorio.saude/observatorio.saude.csproj"
COPY . .
WORKDIR "/src/observatorio.saude"
RUN dotnet build "./observatorio.saude.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./observatorio.saude.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app

# --- ALTERAÇÕES AQUI ---

# Instala o 'curl' para ser usado no health check. É necessário rodar como root.
USER root
RUN apt-get update && apt-get install -y curl && apt-get clean && rm -rf /var/lib/apt/lists/*
# Volta para o usuário da aplicação definido na imagem base
USER $APP_UID

COPY --from=publish /app/publish .

# Verifica a saúde da aplicação a cada 30 segundos
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/api/v1/Health || exit 1

ENTRYPOINT ["dotnet", "observatorio.saude.dll"]