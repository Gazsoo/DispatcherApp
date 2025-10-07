# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

DispatcherApp is a full-stack application with a .NET 9 backend API and React/TypeScript frontend. The application manages assignments, tutorials, and files with role-based authorization (Dispatcher, Administrator roles).

## Architecture

### Backend Architecture (.NET 9)

The backend follows a layered architecture with clear separation of concerns:

- **DispatcherApp.API** - Web API layer with controllers, middleware, and API configuration
- **DispatcherApp.BLL** (Business Logic Layer) - Services, interfaces, DTOs mapping, business logic
- **DispatcherApp.DAL** (Data Access Layer) - Entity Framework Core DbContext, migrations, data seeding
- **DispatcherApp.Models** - Shared models, DTOs, entities, constants, exceptions, configurations

**Dependency flow**: API → BLL → DAL → Models (each layer references only the layer below)

### Frontend Architecture (React + TypeScript)

Located in `src/frontend-app/`, using:
- React 19 with TypeScript
- React Router v7 for routing
- Tailwind CSS v4 for styling
- Vite for build tooling
- Axios for API communication

**Structure**:
- `components/dashboard/` - Dashboard layout and page components
- `components/context/` - React Context providers (AppProviders wraps UserProvider)
- `components/hooks/` - Custom React hooks (useTutorials, useProfile, etc.)
- `components/ui/` - Reusable UI components
- `api/` - API client configuration
- `auth/` - Authentication utilities
- `routes.tsx` - Centralized routing configuration with nested routes

### Key Patterns

**Backend Dependency Injection**: All layers use extension methods on `IHostApplicationBuilder`:
- `AddCommonConfiguration()` - Common app settings (Models layer)
- `AddDataAccessServices()` - DbContext, Identity, health checks (DAL layer)
- `AddBusinessLogicServices()` - Services, JWT auth, AutoMapper (BLL layer)
- `AddApiServices()` - Controllers, CORS, OpenAPI/Swagger, exception handling (API layer)

These are called in sequence in `Program.cs`.

**Authentication**: JWT-based authentication with ASP.NET Core Identity. Frontend stores tokens and includes them in API requests via axios interceptors.

**Database**: SQL Server with Entity Framework Core. AppDbContext includes: Assignments, AssignmentUsers, Tutorials, Files.

## Development Commands

### Backend (.NET)

Build the solution:
```bash
dotnet build DispatcherApp.sln
```

Run the API (from root):
```bash
dotnet run --project src/DispatcherApp.API/DispatcherApp.API.csproj
```

Run tests:
```bash
dotnet test
```

Entity Framework migrations (from root):
```bash
# Create migration
dotnet ef migrations add <MigrationName> --project src/DispatcherApp.DAL --startup-project src/DispatcherApp.API

# Update database
dotnet ef database update --project src/DispatcherApp.DAL --startup-project src/DispatcherApp.API
```

### Frontend (React)

All commands run from `src/frontend-app/` directory:

```bash
# Install dependencies
npm install

# Development server (default port 5173, configurable via VITE_APP_PORT env var)
npm run dev

# Production build (runs TypeScript compiler then Vite build)
npm run build

# Lint
npm run lint

# Preview production build
npm run preview
```

## Important Configuration

- **Connection String**: Set in `src/DispatcherApp.API/appsettings.json` or `appsettings.Development.json` under `ConnectionStrings:DispatcherDb`
- **JWT Settings**: Configured in BLL layer via `JwtSettings` section
- **CORS**: Configured in API layer via `CorsSettings:AllowedOrigins`
- **File Storage**: Configured via `FileStorageSettings` section (currently uses local file storage)

## Project-Specific Notes

- The solution uses **Central Package Management** (see `Directory.Packages.props`)
- **Directory.Build.props** sets .NET 9 as target framework and enables nullable reference types
- All projects treat warnings as errors except NuGet security audit warnings
- The API includes Swagger/OpenAPI at `/api` in development mode
- Health checks available at `/health` endpoint
- Database seeding runs automatically in development (see `RoleSeed()` in Program.cs)
- Frontend proxy/API calls should target the backend API (configure base URL in frontend axios instance)

## Testing

Tests are located in `test/DispatcherApp.Tests/` using xUnit framework with coverlet for code coverage.
