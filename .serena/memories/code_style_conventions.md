# Code Style and Conventions

## General EditorConfig Settings
- **Charset**: UTF-8
- **Indent style**: Spaces (4 spaces for most files)
- **Insert final newline**: true
- **Trim trailing whitespace**: true

## Language-Specific Indentation
- **C# (.cs, .csx, .cake)**: 4 spaces
- **XML/Config files**: 2 spaces
- **JSON files**: 2 spaces
- **YAML files**: 2 spaces
- **Web files (HTML, JS, TS, CSS)**: 2 spaces
- **Solution files (.sln)**: Tab indentation

## .NET/C# Style Guidelines
- **Variable declarations**: `var` only when type is apparent, otherwise explicit types
- **Access modifiers**: Always specify (public, private, etc.)
- **Field naming**: Private fields use camelCase with `_` prefix (e.g., `_fieldName`)
- **Method/Property naming**: PascalCase
- **Interface naming**: PascalCase with `I` prefix (e.g., `IMyInterface`)
- **Generic type parameters**: PascalCase with `T` prefix (e.g., `TEntity`)
- **Constants**: PascalCase for public/protected, camelCase for private
- **Using directives**: Outside namespace, system directives first
- **Braces**: Always use braces for code blocks
- **New lines**: Before else, catch, finally, and open braces

## Frontend/TypeScript Conventions
- **Node.js**: Version 22+ required
- **npm**: Version 10.9+ required
- **Package manager**: npm (not yarn or pnpm)
- **Module system**: ES modules
- **Web components**: LitElement-based
- **Testing**: Web Test Runner with testing library
- **E2E Testing**: Playwright

## File Headers
All C# files should include copyright header:
```
Copyright (c) Umbraco.
See LICENSE for more details.
```