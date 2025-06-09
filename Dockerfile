FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/RotaViagem.API/RotaViagem.API.csproj", "src/RotaViagem.API/"]
COPY ["src/RotaViagem.Application/RotaViagem.Application.csproj", "src/RotaViagem.Application/"]
COPY ["src/RotaViagem.Domain/RotaViagem.Domain.csproj", "src/RotaViagem.Domain/"]
COPY ["src/RotaViagem.Infrastructure/RotaViagem.Infrastructure.csproj", "src/RotaViagem.Infrastructure/"]
COPY ["src/RotaViagem.DependencyInjection/RotaViagem.DependencyInjection.csproj", "src/RotaViagem.DependencyInjection/"]
RUN dotnet restore "src/RotaViagem.API/RotaViagem.API.csproj"
COPY . .
WORKDIR "/src/src/RotaViagem.API"
RUN dotnet build "RotaViagem.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RotaViagem.API.csproj" -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app

# Instala netcat para verificar conectividade
RUN apt-get update && apt-get install -y netcat-traditional && rm -rf /var/lib/apt/lists/*

COPY --from=build /src/src/RotaViagem.Infrastructure /app/src/RotaViagem.Infrastructure
COPY --from=build /src/src/RotaViagem.API /app/src/RotaViagem.API
COPY --from=publish /app/publish ./
COPY entrypoint.sh ./entrypoint.sh
RUN chmod +x ./entrypoint.sh
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
ENTRYPOINT ["./entrypoint.sh"]
