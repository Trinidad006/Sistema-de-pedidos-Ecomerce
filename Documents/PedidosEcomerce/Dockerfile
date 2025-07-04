# Utiliza una imagen oficial de .NET para construir la app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia los archivos del proyecto y restaura dependencias
COPY . .
RUN dotnet restore "ECommerceSystem.Api.csproj"
RUN dotnet publish "ECommerceSystem.Api.csproj" -c Release -o /app/publish

# Utiliza una imagen más ligera para ejecutar la app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expone el puerto por defecto de Kestrel
EXPOSE 80

# Comando para ejecutar la app
ENTRYPOINT ["dotnet", "ECommerceSystem.Api.dll"] 