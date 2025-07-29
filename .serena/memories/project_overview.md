# Umbraco CMS Project Overview

## Purpose
Umbraco CMS is a free and open source .NET content management system. The mission is to help deliver delightful digital experiences by making Umbraco friendly, simpler and social.

## Tech Stack
- **.NET 9.0**: Main target framework (see global.json)
- **ASP.NET Core**: Web framework
- **Entity Framework Core**: Database ORM option
- **NPoco**: Micro ORM used extensively
- **NUnit**: Testing framework for C#
- **TypeScript**: Frontend language
- **LitElement**: Web components library for frontend
- **Vite**: Frontend build tool
- **Playwright**: E2E testing
- **Node.js 22+** and **npm 10.9+**: Frontend tooling requirements

## Key Architecture Components
### Backend (.NET)
- **Umbraco.Core**: Fundamental types, interfaces, and core functionality
- **Umbraco.Infrastructure**: Data access, services, and infrastructure concerns
- **Umbraco.Cms**: Main CMS package that ties everything together
- **Umbraco.Web.UI**: ASP.NET Core web application hosting the CMS
- **Umbraco.Cms.Api.Management**: Management/backoffice API endpoints
- **Umbraco.Cms.Api.Delivery**: Content delivery API for frontend consumption

### Frontend (TypeScript/LitElement)
- **Umbraco.Web.UI.Client**: Main TypeScript/LitElement frontend application (backoffice)
- **Umbraco.Web.UI.Login**: Separate TypeScript application for login screen
- Uses modular architecture with Extension API, Context API, Observable API, Element API
- Web components using LitElement for UI elements

### Database Support
- SQL Server and SQLite providers
- Both direct and Entity Framework Core implementations