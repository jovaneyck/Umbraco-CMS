# Codebase Structure

## Root Directory Structure
```
/
├── src/                    # Source code
├── tests/                  # Test projects
├── build/                  # Build artifacts
├── tools/                  # Development tools
├── templates/              # Project templates
├── .github/                # GitHub workflows and templates
├── .vscode/                # VS Code configuration
├── .devcontainer/          # Dev container configuration
├── umbraco.sln             # Main solution file
├── global.json             # .NET SDK version
├── Directory.Build.props   # MSBuild properties
├── Directory.Packages.props # NuGet package versions
└── CLAUDE.md              # Development guide
```

## Source Projects Structure (/src)
### Core Backend Components
- **Umbraco.Core/**: Fundamental types and interfaces
- **Umbraco.Infrastructure/**: Data access and services
- **Umbraco.Cms/**: Main CMS package
- **Umbraco.Web.UI/**: ASP.NET Core web application
- **Umbraco.Web.Common/**: Shared web utilities
- **Umbraco.Web.Website/**: Website-specific functionality

### API Components
- **Umbraco.Cms.Api.Common/**: Shared API utilities
- **Umbraco.Cms.Api.Management/**: Backoffice API
- **Umbraco.Cms.Api.Delivery/**: Content delivery API

### Database Persistence
- **Umbraco.Cms.Persistence.SqlServer/**: SQL Server provider
- **Umbraco.Cms.Persistence.Sqlite/**: SQLite provider
- **Umbraco.Cms.Persistence.EFCore/**: Entity Framework integration
- **Umbraco.Cms.Persistence.EFCore.SqlServer/**: EF Core SQL Server
- **Umbraco.Cms.Persistence.EFCore.Sqlite/**: EF Core SQLite

### Frontend Applications
- **Umbraco.Web.UI.Client/**: Main backoffice TypeScript app
- **Umbraco.Web.UI.Login/**: Login screen TypeScript app
- **Umbraco.Web.UI.Docs/**: Documentation site

### Additional Components
- **Umbraco.Examine.Lucene/**: Search functionality
- **Umbraco.Cms.Imaging.ImageSharp/**: Image processing
- **Umbraco.PublishedCache.HybridCache/**: Caching system
- **Umbraco.Cms.StaticAssets/**: Static asset management

## Test Projects Structure (/tests)
- **Umbraco.Tests.UnitTests/**: Unit tests (NUnit)
- **Umbraco.Tests.Integration/**: Integration tests
- **Umbraco.Tests.Common/**: Shared test utilities
- **Umbraco.Tests.Benchmarks/**: Performance benchmarks
- **Umbraco.Tests.AcceptanceTest/**: End-to-end acceptance tests