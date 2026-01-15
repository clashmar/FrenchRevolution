# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy everything
COPY . .

# Restore dependencies
RUN dotnet restore FrenchRevolutionApi.sln

# Build & publish the API project
RUN dotnet publish src/FrenchRevolution.Application/FrenchRevolution.Application.csproj \
    -c Debug -o /app --no-restore

# Runtime stage 
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app ./
EXPOSE 80
ENTRYPOINT ["dotnet", "FrenchRevolution.Application.dll"]   