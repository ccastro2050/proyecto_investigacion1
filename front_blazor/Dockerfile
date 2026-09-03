# ==============================================================
# Front Blazor — Investigación · Blazor Server / .NET 10 (puerto 8071)
#
# Usa la imagen del SDK (no la de runtime) a propósito: el contenedor corre
# `dotnet watch`, que RECOMPILA Y REINICIA solo al guardar un .razor o un .cs.
# Es el mismo ciclo que la API.
# ==============================================================
FROM mcr.microsoft.com/dotnet/sdk:10.0

WORKDIR /app

# Primero solo el .csproj: si no cambian las dependencias, Docker reutiliza
# la capa del restore y los arranques siguientes son más rápidos.
COPY FrontInvestigacion.csproj ./
RUN dotnet restore FrontInvestigacion.csproj

COPY . .

# DOTNET_USE_POLLING_FILE_WATCHER: en Docker sobre Windows los avisos de
#   "archivo cambiado" no siempre llegan al contenedor. El sondeo revisa cada
#   tanto y el watch funciona de verdad.
ENV DOTNET_USE_POLLING_FILE_WATCHER=true \
    ASPNETCORE_ENVIRONMENT=Development \
    ASPNETCORE_URLS=http://0.0.0.0:8071

EXPOSE 8071

# --no-launch-profile ignora launchSettings.json y respeta ASPNETCORE_URLS.
CMD ["dotnet", "watch", "--project", "FrontInvestigacion.csproj", "run", "--no-launch-profile"]
