# GameStore

A full-stack game store application built with ASP.NET Core. Currently focused on backend development with plans for a frontend implementation.

## Tech Stack

### Backend (In Progress)
- **.NET 9**
- **ASP.NET Core** - Web API framework
- **Entity Framework Core** - ORM
- **SQLite** - Database
- **Repository Pattern** - Data access layer
- **Result Pattern** - Error handling

### Frontend (Next Phase)
TBD - Considering options like Blazor, React, or other modern frameworks

## Features

- Game management
- User management
- Order processing
- Review system
- Genre and platform categorization

## Getting Started

### Prerequisites
- .NET 9 SDK
- SQLite

### Running the API

```bash
# Navigate to the API project
cd src/GameStore.API

# Restore dependencies
dotnet restore

# Run the application
dotnet run
```

The API will be available at `https://localhost:7000` or `http://localhost:5000`

## Development

Built on **Arch Linux** with **.NET 9**

```bash
# Run the application
dotnet run

# Create a new migration
dotnet ef migrations add MigrationName

# Apply migrations to database
dotnet ef database update

# Build the project
dotnet build
```

## Learning Goals

- Understanding ASP.NET Core Web APIs
- Implementing clean architecture patterns
- Working with Entity Framework Core
- Database design and migrations
- RESTful API best practices
- Full-stack development (frontend TBD)

## Roadmap

- [x] Project setup and architecture
- [x] Database design and Entity Framework configuration
- [x] Repository pattern implementation
- [ ] Complete API endpoints
- [ ] Service layer implementation
- [ ] API testing and validation
- [ ] Frontend development (framework TBD)
- [ ] Integration and deployment

## Status

🚧 **Work in Progress** - Currently building the backend API

---

