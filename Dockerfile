# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy all csproj files and restore
COPY ["KaappaanPlus.WebApi/KaappaanPlus.WebApi.csproj", "KaappaanPlus.WebApi/"]
COPY ["KaappaanPlus.Application/KaappaanPlus.Application.csproj", "KaappaanPlus.Application/"]
COPY ["KaappaanPlus.Domain/KaappaanPlus.Domain.csproj", "KaappaanPlus.Domain/"]
COPY ["KaappaanPlus.Infrastructure/KaappaanPlus.Infrastructure.csproj", "KaappaanPlus.Infrastructure/"]
COPY ["KaappanPlus.Persistence/KaappanPlus.Persistence.csproj", "KaappanPlus.Persistence/"]

RUN dotnet restore "KaappaanPlus.WebApi/KaappaanPlus.WebApi.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/KaappaanPlus.WebApi"
RUN dotnet build "KaappaanPlus.WebApi.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "KaappaanPlus.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KaappaanPlus.WebApi.dll"]
