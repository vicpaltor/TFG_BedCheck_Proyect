# -----------------------------------------------------------------------
# FASE 1: BUILD (Compilación)
# -----------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto (.csproj)
COPY *.sln .
COPY BedCheck.AccesoDatos/*.csproj BedCheck.AccesoDatos/
COPY BedCheck.Models/*.csproj BedCheck.Models/
COPY BedCheck.Servicios/*.csproj BedCheck.Servicios/
COPY BedCheck.Tests/*.csproj BedCheck.Tests/
COPY BedCheck.Utilidades/*.csproj BedCheck.Utilidades/
COPY BedCheck/*.csproj BedCheck/

# Restaurar dependencias
RUN dotnet restore

# Copia el resto del código
COPY . .

# Publicar la aplicación principal
RUN dotnet publish "BedCheck/BedCheck.csproj" -c Release -o /app/publish

# -----------------------------------------------------------------------
# FASE 2: FINAL (Ejecución)
# -----------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copia solo el resultado publicado
COPY --from=build /app/publish .

# Configuración y puerto
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "BedCheck.dll"]