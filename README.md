# **Loyalty System API**

A .NET 8 API for a loyalty system demo with endpoints to earn points . 
The project follows the Onion Architecture and uses code-first database migration. 
The application is configured with JWT authentication, Swagger, Redis cache, and integrates with Keycloak for authentication.

## Project Structure
The project is divided into the following layers:

LoyaltySystemAPI: The main API project with controllers and configurations.
LoyaltySystemApplication: Contains business logic service interfaces and implementations.
LoyaltySystemCore: Contains entities and domain models.
LoyaltySystemInfrastructures: Infrastructure layer for data access and external dependencies.
LoyaltySystemTest: Contains unit and integration tests for the application.

## Features

Earn points endpoint.

Code-first database with initial seed data.

JWT authentication

Swagger integration

Redis caching

Keycloak integration for authentication

## Prerequisites

.NET 8 SDK

SQL Server

Redis

Keycloak

## Setup Instructions

1-Clone the repository:

			git clone https://github.com/your-username/loyalty-system.git
			cd loyalty-system
2-Update connection strings:

Update the appsettings.json file in the LoyaltySystemAPI project with your SQL Server and Redis connection strings:

		{
		  "ConnectionStrings": {
		    "LoyaltySystemDB": "Your SQL Server connection string",
		    "RedisConnection": "Your Redis connection string"
		  },
		  "Keycloak": {
		    "Authority": "Your Keycloak authority URL",
		    "ClientId": "Your Keycloak client ID",
		    "ClientSecret": "Your Keycloak client secret"
		  }
		}

3-Run database migrations:

dotnet ef migrations add InitialCreate --project LoyaltySystemInfrastructures
dotnet ef database update --project LoyaltySystemInfrastructures

4-Run the application:

dotnet run --project LoyaltySystemAPI

## Authentication
The API uses JWT authentication with Keycloak. Ensure that your Keycloak configuration is correctly set up in the appsettings.json file.

## Logging
The application uses Serilog for logging. Logs will be written to the configured sinks as per the appsettings.json file.

