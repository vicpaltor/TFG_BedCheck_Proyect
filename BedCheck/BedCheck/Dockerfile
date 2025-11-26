# 1. IMAGEN BASE (SDK para compilar)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 2. COPIAR ARCHIVOS DE PROYECTO (Para restaurar dependencias)
# Copiamos csproj de todas las capas
COPY ["BedCheck/BedCheck.csproj", "BedCheck/"]
COPY ["BedCheck.AccesoDatos/BedCheck.AccesoDatos.csproj", "BedCheck.AccesoDatos/"]
COPY ["BedCheck.Models/BedCheck.Models.csproj", "BedCheck.Models/"]
COPY ["BedCheck.Utilidades/BedCheck.Utilidades.csproj", "BedCheck.Utilidades/"]
COPY ["BedCheck.Servicios/BedCheck.Servicios.csproj", "BedCheck.Servicios/"]

# 3. RESTAURAR DEPENDENCIAS
RUN dotnet restore "BedCheck/BedCheck.csproj"

# 4. COPIAR EL RESTO DEL CÓDIGO Y COMPILAR
COPY . .
WORKDIR "/src/BedCheck"
RUN dotnet build "BedCheck.csproj" -c Release -o /app/build

# 5. PUBLICAR (Generar los archivos finales)
FROM build AS publish
RUN dotnet publish "BedCheck.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 6. IMAGEN FINAL (Runtime ligero para ejecutar)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BedCheck.dll"]