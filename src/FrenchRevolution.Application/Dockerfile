FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/FrenchRevolution.Application/FrenchRevolution.Application.csproj", "src/FrenchRevolution.Application/"]
COPY ["src/FrenchRevolution.Contracts/FrenchRevolution.Contracts.csproj", "src/FrenchRevolution.Contracts/"]
COPY ["src/FrenchRevolution.Domain/FrenchRevolution.Domain.csproj", "src/FrenchRevolution.Domain/"]
COPY ["src/FrenchRevolution.Infrastructure/FrenchRevolution.Infrastructure.csproj", "src/FrenchRevolution.Infrastructure/"]
RUN dotnet restore "src/FrenchRevolution.Application/FrenchRevolution.Application.csproj"
COPY . .
WORKDIR "/src/src/FrenchRevolution.Application"
RUN dotnet build "./FrenchRevolution.Application.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FrenchRevolution.Application.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FrenchRevolution.Application.dll"]
