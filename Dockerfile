# Use the official ASP.NET Core runtime image for .NET 8
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official .NET SDK image for .NET 8 to build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file and csproj files for dependency restoration
COPY LoyaltySystem.sln ./
COPY LoyaltySystemAPI/LoyaltySystemAPI.csproj LoyaltySystemAPI/
COPY LoyaltySystemApplication/LoyaltySystemApplication.csproj LoyaltySystemApplication/
COPY LoyaltySystemDomain/LoyaltySystemDomain.csproj LoyaltySystemDomain/
COPY LoyaltySystemInfrastructures/LoyaltySystemInfrastructures.csproj LoyaltySystemInfrastructures/
COPY LoyaltySystemTests/LoyaltySystemTest.csproj LoyaltySystemTests/

# Restore dependencies
RUN dotnet restore

# Copy the entire solution and build the project
COPY . .
WORKDIR /src/LoyaltySystemAPI
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 5000
ENTRYPOINT ["dotnet", "LoyaltySystemAPI.dll"]