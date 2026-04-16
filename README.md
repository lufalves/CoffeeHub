# CoffeeHub

CoffeeHub is a personal project focused on learning and practicing C#, ASP.NET Core, Clean Architecture, domain modeling, testing, and software engineering best practices.

The product idea is a coffee platform inspired by apps such as Vivino, but focused on coffee. Users can register coffees, review them, save recipes, and explore information about roasteries, origins, farms, and brewing methods.

## Solution Overview

This solution is organized into:

| Project | Layer | Description |
|---|---|---|
| `CoffeeHub.Domain` | Domain | Entities, value objects, and domain rules |
| `CoffeeHub.Domain.Common` | Domain Shared | Base entity class, interfaces, and value objects shared across domain |
| `CoffeeHub.Application` | Application | Use cases, service interfaces, DTOs, and orchestration |
| `CoffeeHub.Infrastructure` | Infrastructure | EF Core, PostgreSQL, repository implementations |
| `CoffeeHub.Api` | API | RESTful API with JWT Bearer authentication |
| `CoffeeHub.Web` | Web | Razor Pages UI with Cookie authentication and i18n |
| `CoffeeHub.Tests` | Tests | Unit and integration tests (xUnit) |

The solution file is:

- `CoffeeHub.sln`

## Architecture

Current architectural flow:

```
Api/Web -> Application Services -> Repository Interfaces -> Infrastructure Repositories -> DbContext -> PostgreSQL
```

Authentication flows:

- **Web**: `Razor Page -> AuthService -> IUserRepository -> PostgreSQL -> Cookie Authentication`
- **API**: `Controller -> AuthService -> JWT Bearer Token`

## Main Conventions

- Code is written in English.
- Entity names use singular form.
- IDs use `Guid`.
- Audit fields use `DateTimeOffset`.
- Soft delete is preferred over physical delete.
- Controllers and Razor Pages should not contain business rules.
- Application services validate and orchestrate use cases.
- Repositories focus on persistence operations.
- Infrastructure contains EF Core, PostgreSQL, mappings, hashing, and repository implementations.

## Current Domain Scope

The domain currently includes:

| Entity | Description |
|---|---|
| `User` | Platform users with roles (Admin/User) |
| `Coffee` | Coffee catalog entries with barcode, roastery, origin, bean variety, roast level |
| `Review` | User reviews and ratings for coffees |
| `Recipe` | Brewing recipes with methods and instructions |
| `Roastery` | Coffee roastery information |
| `CoffeeShop` | Coffee shop reference data |
| `Origin` | Coffee origin country/region |
| `Farm` | Coffee farm linked to an origin |
| `BeanVariety` | Coffee bean variety reference data |
| `RoastLevel` | Roast level reference data |
| `BrewingMethod` | Brewing method reference data |

Domain value objects:

- `BarcodeValue` - Encapsulates barcode validation
- `EmailAddress` - Encapsulates email validation
- `UrlValue` - Encapsulates URL validation

Important domain rules:

- `Coffee.Barcode` is required and unique
- First registered user is automatically assigned Admin role
- Soft delete via `IsDeleted` flag on all entities
- All entities inherit from `EntityBase` with audit fields

## Current State

The project already has:

### Authentication & Authorization
- Cookie-based login and registration in `CoffeeHub.Web`
- JWT Bearer authentication in `CoffeeHub.Api`
- Role-based authorization (Admin/User)
- Brute-force login protection (5 attempts, 15-min lockout)
- Password hashing via ASP.NET Core PasswordHasher
- Profile management (update name, email, avatar, change password)

### Web Application (Razor Pages)
- Authenticated home page with paginated coffee feed
- Coffee CRUD pages (create, edit, details, delete)
- Recipe CRUD pages
- Review CRUD pages
- Roastery listing and detail pages
- Origin listing and detail pages
- Profile page with user role display
- Error and 404 not found pages
- Internationalization (i18n) with English and Portuguese (PT-BR) support
- Language switcher in navbar with cookie-based persistence

### REST API
- CRUD endpoints for all entities (Users, Coffees, Recipes, Reviews, Roasteries, Origins, Farms, BeanVarieties, BrewingMethods, RoastLevels)
- Auth controller with login, register, and token refresh
- JWT Bearer authentication on all endpoints
- Request/Response DTOs with mapping layer
- Duplicate constraint handling (409 Conflict)

### Data Layer
- PostgreSQL configured and running locally
- EF Core with migrations applied
- Eager loading for Coffee details (5 navigation properties)
- Pagination support for listings
- Unique constraints and indexes for frequently queried fields
- Seed data for reference tables

### Testing
- xUnit tests for UserService, CoffeeService, and AuthService
- Test data builders and fake repositories
- Integration test infrastructure with PostgreSQL
- coverlet for code coverage

### Documentation
- README.md files in each project
- Architecture decisions in `docs/decisions/`
- Domain documentation in `docs/domain/`
- Use case documentation in `docs/use-cases/`

## Project Rules

- `CoffeeHub.Domain`: entities, value objects, and domain concepts only
- `CoffeeHub.Domain.Common`: shared base classes, interfaces, and value objects
- `CoffeeHub.Application`: interfaces, services, validation, authentication flow, use case orchestration
- `CoffeeHub.Infrastructure`: EF Core, `DbContext`, mappings, password hashing, repository implementations
- `CoffeeHub.Api`: REST endpoints, JWT auth, DTOs, and HTTP concerns only
- `CoffeeHub.Web`: Razor Pages UI, cookie auth, i18n, and presentation concerns only
- `CoffeeHub.Tests`: unit and integration tests for application behavior

## What To Avoid

- Do not place EF Core code inside `Domain`.
- Do not place persistence logic inside controllers or pages.
- Do not place direct database access inside `Application` services.
- Do not use `Api` or `Web` as the place for business rules.
- Do not mix naming conventions or switch between Portuguese and English in code.

## Next Recommended Steps

- Add "Remember Me" checkbox with extended cookie expiration
- Add lockout remaining time display on Login page
- Add "Forgot Password" page with email-based reset flow
- Add client-side validation with jQuery Validate on forms
- Add confirmation dialog before delete on Details pages
- Display reviews and recipes on coffee detail page
- Display average rating on coffee detail page
- Add filtering support to coffee listing (roast level, origin, bean variety)
- Add search by name or barcode
- Add integration tests for repository persistence
- Set up code coverage reporting with target of 80%
- Add OpenAPI/Swagger documentation for the API
- Add API versioning

### Localization (i18n)

- Objective: Add English and Portuguese UI support with a language switcher that persists the user's choice in a cookie.
- Implementation:
  - Official ASP.NET Core localization with .resx resources (SharedResources and Portuguese translations).
  - Data Annotations localization for validation messages via FormModels using `ErrorMessage` keys explicitly.
  - Form validation with `IStringLocalizer` dependency injection inside PageModels mapped directly down to Form logic validation logic.
  - Cookie-based persistence for culture preference (`.CoffeeHub.Culture`).
  - Navbar language switcher partial that toggles between EN and PT-BR.
  - Dynamic language selection in the main layout and all Razor Pages via IStringLocalizer and IViewLocalizer.
  - Program.cs wired to register localization services and middleware; a CultureController to handle culture changes.
- Key files:
  - `CoffeeHub.Web/Resources/SharedResources.resx` and `SharedResources.pt.resx`
  - `CoffeeHub.Web/Controllers/CultureController.cs`
  - `CoffeeHub.Web/Pages/Shared/_LanguageSwitcher.cshtml`
  - `CoffeeHub.Web/Pages/Shared/_Layout.cshtml`
  - FormModels (e.g. `CoffeeFormModel.cs`, `RecipeFormModel.cs`) using `[Required(ErrorMessage = "Key")]` 
  - `CoffeeHub.Web/Program.cs` 
- How to test:
  1. Build and run the app.
  2. On any page, use the language switcher in the navbar to switch between English and Portuguese.
  3. Verify texts update instantly (e.g., navbar items, headers, footers, tables and labels).
  4. Submit invalid forms to check validation error messages displaying natively in the chosen culture.
  5. Refresh the page and ensure the language persists (cookie-based persistence).
- Future improvements:
  - Add OpenAPI documentation for the API if localization for API endpoints is desired later.
  - Localize email sending templates.
