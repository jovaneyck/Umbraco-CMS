# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository Overview

Umbraco CMS is a .NET-based content management system with a TypeScript/LitElement frontend. This is a complex, multi-project solution supporting multiple database providers and containing both backend APIs and frontend applications.

## Key Commands

### Building and Running
```bash
# Build the entire solution
dotnet build

# Run the main web application for development
cd src/Umbraco.Web.UI
dotnet run

# Run without rebuilding (after initial build)
dotnet run --no-build

# Build NuGet packages for distribution
dotnet pack -c Release -o Build.Out
```

### Frontend Development
```bash
# Frontend development (Client UI) - run in separate terminal
cd src/Umbraco.Web.UI.Client
npm ci --no-fund --no-audit --prefer-offline
npm run dev:server

# Build frontend for CMS integration
npm run build:for:cms

# Login screen development
cd src/Umbraco.Web.UI.Login
npm run build

# Update client API types after backend API changes
cd src/Umbraco.Web.UI.Client
npm run generate:server-api
```

### Testing
```bash
# Run unit tests
dotnet test tests/Umbraco.Tests.UnitTests/

# Run integration tests
dotnet test tests/Umbraco.Tests.Integration/

# Run frontend unit tests
cd src/Umbraco.Web.UI.Client
npm test

# Run frontend E2E tests
npm run test:e2e

# Frontend test development mode
npm run test:dev-watch
```

### Linting and Code Quality
```bash
# Frontend linting
cd src/Umbraco.Web.UI.Client
npm run lint
npm run lint:fix

# Format frontend code
npm run format
npm run format:fix
```

## Architecture Overview

### Core Structure
- **Umbraco.Core**: Fundamental types, interfaces, and core functionality
- **Umbraco.Infrastructure**: Data access, services, and infrastructure concerns  
- **Umbraco.Cms**: Main CMS package that ties everything together
- **Umbraco.Web.UI**: ASP.NET Core web application hosting the CMS
- **Umbraco.Web.UI.Client**: TypeScript/LitElement frontend application (backoffice)
- **Umbraco.Web.UI.Login**: Separate TypeScript application for login screen

### API Structure
- **Umbraco.Cms.Api.Common**: Shared API utilities and base classes
- **Umbraco.Cms.Api.Management**: Management/backoffice API endpoints
- **Umbraco.Cms.Api.Delivery**: Content delivery API for frontend consumption

### Database Support
- **Umbraco.Cms.Persistence.SqlServer**: SQL Server provider
- **Umbraco.Cms.Persistence.Sqlite**: SQLite provider
- **Umbraco.Cms.Persistence.EFCore**: Entity Framework Core integration
- **Umbraco.Cms.Persistence.EFCore.SqlServer**: EF Core SQL Server support
- **Umbraco.Cms.Persistence.EFCore.Sqlite**: EF Core SQLite support

### Frontend Architecture
The frontend uses a modular architecture with:
- **Extension API**: Plugin/extension system for extending functionality
- **Context API**: State management and dependency injection
- **Observable API**: Reactive state management
- **Element API**: Custom web components using LitElement
- **Localization API**: Multi-language support

Key frontend concepts:
- Extensions and manifests for modularity
- Context providers for dependency injection
- Observable states for reactive programming
- Web components for UI elements

## Development Workflows

### Backend Development
1. Make changes to C# code in relevant projects
2. Build solution: `dotnet build`
3. Run tests: `dotnet test`
4. Start web application: `cd src/Umbraco.Web.UI && dotnet run`

### Frontend Development  
1. Start backend: `cd src/Umbraco.Web.UI && dotnet run --no-build`
2. In separate terminal, start frontend dev server: `cd src/Umbraco.Web.UI.Client && npm run dev:server`
3. Make frontend changes (hot reload enabled)
4. Run frontend tests: `npm test`

### Full-Stack Development
1. When changing both backend APIs and frontend:
2. Update backend code and rebuild
3. Regenerate client API types: `npm run generate:server-api`
4. Update OpenApi.json from `/umbraco/swagger/management/swagger.json`
5. Update frontend code accordingly

## Testing Strategy

### Backend Tests
- **Unit Tests**: `Umbraco.Tests.UnitTests` - Fast, isolated tests using NUnit
- **Integration Tests**: `Umbraco.Tests.Integration` - Database and service integration tests  
- **Benchmarks**: `Umbraco.Tests.Benchmarks` - Performance testing

### Frontend Tests  
- **Unit Tests**: Web Test Runner with testing library
- **E2E Tests**: Playwright for browser automation
- **Storybook**: Component documentation and testing

## Special Configuration

### Frontend Development with Backend
Add to `src/Umbraco.Web.UI/appsettings.json` under `Umbraco:Cms:Security`:
```json
{
  "BackOfficeHost": "http://localhost:5173",
  "AuthorizeCallbackPathName": "/oauth_complete", 
  "AuthorizeCallbackLogoutPathName": "/logout",
  "AuthorizeCallbackErrorPathName": "/error"
}
```

### Disable Frontend Builds During Backend Development
Comment out in `Umbraco.Cms.StaticAssets.csproj`:
```xml
REM npm ci --no-fund --no-audit --prefer-offline
REM npm run build:for:cms
```

## Development Environment Setup

### Prerequisites
- .NET 9.0+ SDK
- Node.js 22+ and npm 10.9+
- Git

### Initial Setup
1. Clone repository
2. Build entire solution: `dotnet build`
3. Install frontend dependencies: `cd src/Umbraco.Web.UI.Client && npm ci`
4. Run: `cd src/Umbraco.Web.UI && dotnet run`
5. Follow installer to set up database

### Cleaning/Reset
To reset development environment:
1. Delete `src/Umbraco.Web.UI/appsettings.json`
2. Delete `src/Umbraco.Web.UI/umbraco/Data` folder
3. Or use: `git clean -xdf .` for complete reset

## Target Framework and Technology Stack

- **.NET 9.0**: Main target framework
- **ASP.NET Core**: Web framework
- **Entity Framework Core**: Database ORM option  
- **NPoco**: Micro ORM used extensively
- **NUnit**: Testing framework
- **TypeScript**: Frontend language
- **LitElement**: Web components library
- **Vite**: Frontend build tool
- **Playwright**: E2E testing
- **Storybook**: Component documentation

## Branch and PR Guidelines

- Target `main` branch for contributions
- Branch naming: `v{major}/{feature|bugfix|task}/{issue}-{description}`
- Example: `v15/bugfix/18132-rte-tinymce-onchange-value-check`