# Essential Commands for Umbraco CMS Development

## Building and Running
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

## Frontend Development
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

## Testing Commands
```bash
# Backend unit tests
dotnet test tests/Umbraco.Tests.UnitTests/

# Backend integration tests  
dotnet test tests/Umbraco.Tests.Integration/

# Frontend unit tests
cd src/Umbraco.Web.UI.Client
npm test

# Frontend E2E tests
npm run test:e2e

# Frontend test development mode
npm run test:dev-watch
```

## Code Quality Commands
```bash
# Frontend linting
cd src/Umbraco.Web.UI.Client
npm run lint
npm run lint:fix

# Format frontend code
npm run format
npm run format:fix

# TypeScript compilation check
npm run compile
```

## System Utilities
- **git**: Version control
- **ls**: List files and directories
- **find**: Search for files
- **grep**: Search text patterns
- **cd**: Change directory