# Dockerfile en la raíz del proyecto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia solo la carpeta de la API
COPY Documents/PedidosEcomerce/ECommerceSystem.Api/ ./ECommerceSystem.Api/
WORKDIR /src/ECommerceSystem.Api

RUN dotnet restore "ECommerceSystem.Api.csproj"
RUN dotnet publish "ECommerceSystem.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "ECommerceSystem.Api.dll"] 