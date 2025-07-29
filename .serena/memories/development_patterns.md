# Development Patterns and Guidelines

## Design Principles
- **Test-Driven Development (TDD)**: Always write failing tests first, then implement
- **4 Rules of Simple Design**: Code that passes tests, reveals intent, minimizes duplication, has minimal elements
- **SOLID Principles**: Applied throughout the codebase
- **Object-Oriented and Functional Design**: Mixed paradigm approach

## Key Development Patterns

### Backend Patterns
- **Repository Pattern**: Used for data access abstraction
- **Dependency Injection**: ASP.NET Core DI container
- **Event-Driven Architecture**: Notification handlers and events
- **Service Layer Pattern**: Business logic separation
- **Entity Framework + NPoco**: Dual ORM approach

### Frontend Patterns
- **Web Components**: LitElement-based custom elements
- **Extension System**: Modular plugin architecture
- **Context API**: Dependency injection for frontend
- **Observable Pattern**: Reactive state management
- **Manifest System**: Configuration-driven functionality

## Code Organization Guidelines
- **Namespace Structure**: Matches project/folder structure
- **Single Responsibility**: Each class has one clear purpose
- **Interface Segregation**: Small, focused interfaces
- **Composition over Inheritance**: Favor composition patterns

## API Design Patterns
- **RESTful APIs**: Standard HTTP methods and status codes
- **OpenAPI/Swagger**: API documentation and client generation
- **Versioning**: API version management
- **Response Models**: Consistent API response structures

## Testing Patterns
- **Unit Tests**: Fast, isolated, using NUnit
- **Integration Tests**: Database and service integration
- **Acceptance Tests**: End-to-end user scenarios
- **Benchmarks**: Performance testing
- **Frontend Tests**: Web Test Runner + Playwright

## Error Handling
- **Exception Handling**: Structured exception types
- **Logging**: Microsoft.Extensions.Logging
- **Validation**: Data annotations and custom validators
- **User-Friendly Messages**: Localized error messages

## Security Patterns
- **Authentication**: ASP.NET Core Identity
- **Authorization**: Role and policy-based
- **Input Validation**: Server-side validation
- **XSS Protection**: Content sanitization
- **CSRF Protection**: Anti-forgery tokens