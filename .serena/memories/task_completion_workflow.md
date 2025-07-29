# Task Completion Workflow

## When a Development Task is Completed

### 1. Backend Development Tasks
After making C# code changes:

```bash
# Build the solution to check for compilation errors
dotnet build

# Run relevant unit tests
dotnet test tests/Umbraco.Tests.UnitTests/

# Run integration tests if changes affect data layer
dotnet test tests/Umbraco.Tests.Integration/

# Verify the web application still runs
cd src/Umbraco.Web.UI
dotnet run --no-build
```

### 2. Frontend Development Tasks
After making TypeScript/frontend changes:

```bash
cd src/Umbraco.Web.UI.Client

# Check TypeScript compilation
npm run compile

# Run linting and fix issues
npm run lint:fix

# Format code
npm run format:fix

# Run unit tests
npm test

# If API changes were made, regenerate client API types
npm run generate:server-api
```

### 3. Full-Stack Development Tasks
When changes affect both backend and frontend:

```bash
# 1. Build backend first
dotnet build

# 2. Regenerate API types if needed
cd src/Umbraco.Web.UI.Client
npm run generate:server-api

# 3. Run frontend checks
npm run compile
npm run lint:fix
npm run format:fix
npm test

# 4. Run backend tests
cd ../..
dotnet test tests/Umbraco.Tests.UnitTests/
```

### 4. Before Submitting Changes
Always ensure:
- All tests pass (both backend and frontend)
- Code compiles without errors
- Code follows project style guidelines
- No linting errors remain
- Application runs successfully

### 5. Important Notes
- NEVER commit changes without running tests first
- Always follow the Test-Driven Development approach when possible
- Fix any build or test failures before considering the task complete
- Use `dotnet run --no-build` to avoid unnecessary rebuilds during testing