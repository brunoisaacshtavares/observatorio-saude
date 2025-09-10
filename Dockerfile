# Estágio de imagem base (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# --- Estágio de build do .NET ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["observatorio.saude/observatorio.saude.csproj", "observatorio.saude/"]
RUN dotnet restore "./observatorio.saude/observatorio.saude.csproj"
COPY . .
WORKDIR "/src/observatorio.saude"
RUN dotnet build "./observatorio.saude.csproj" -c $BUILD_CONFIGURATION -o /app/build

# --- ✅ ADICIONE A LINHA ABAIXO ---
# Instala a ferramenta 'dotnet-ef' no diretório /tools DENTRO deste estágio
RUN dotnet tool install dotnet-ef --tool-path /tools


# --- Estágio de publish do .NET ---
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./observatorio.saude.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


# --- Estágio final (runtime) ---
FROM base AS final
WORKDIR /app

USER root

# Instala dependências do Python
RUN apt-get update && \
    apt-get install -y --no-install-recommends python3 python3-pip python3-venv curl && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Cria e ativa o ambiente virtual do Python
RUN python3 -m venv /opt/venv
ENV PATH="/opt/venv/bin:$PATH"

# Copia e instala as dependências do Python
COPY --from=build /src/observatorio.saude/Scripts/EtlEstabelecimentos/requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

# Copia os arquivos da aplicação
COPY --from=build /src/observatorio.saude/Scripts/EtlEstabelecimentos/cnes.py .
COPY --from=publish /app/publish .

USER $APP_UID

HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/api/v1/Health || exit 1

ENTRYPOINT ["dotnet", "observatorio.saude.dll"]