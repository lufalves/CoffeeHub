# CoffeeHub

CoffeeHub is a personal project focused on learning and practicing C#, ASP.NET Core, application architecture, domain modeling, testing, and general software engineering best practices.

The product idea is a coffee platform inspired by apps such as Vivino, but focused on coffee. Users can register coffees, review them, save recipes, and explore information about roasteries, origins, farms, and brewing methods.

## Solution Overview

This solution is currently organized into:

- `CoffeeHub.Domain`
- `CoffeeHub.Application`
- `CoffeeHub.Infrastructure`
- `CoffeeHub.Api`
- `CoffeeHub.Web`
- `CoffeeHub.Tests`

The solution file is:

- `CoffeeHub.sln`

## Architecture

Current architectural flow:

`Api/Web -> Application Services -> Repository Interfaces -> Infrastructure Repositories -> DbContext -> PostgreSQL`

Authentication flow in the web application:

`Razor Page -> AuthService -> IUserRepository -> PostgreSQL -> Cookie Authentication`

## Main Conventions

- Code is written in English.
- Entity names use singular form.
- IDs use `Guid`.
- Audit fields use `DateTimeOffset`.
- Soft delete is preferred over physical delete.
- Controllers and Razor Pages should not contain business rules.
- Application services should validate and orchestrate use cases.
- Repositories should focus on persistence operations.
- Infrastructure should contain EF Core, PostgreSQL, mappings, hashing, and repository implementations.

## Current Domain Scope

The initial domain currently includes:

- `User`
- `Coffee`
- `Review`
- `Recipe`
- `BrewingMethod`
- `Roastery`
- `CoffeeShop`
- `Origin`
- `Farm`
- `BeanVariety`
- `RoastLevel`

Important current rule:

- `Coffee.Barcode` is required
- `Coffee.Barcode` must be unique

## Current State

The project already has:

- PostgreSQL configured and running locally
- EF Core migrations applied
- cookie-based login and registration in `CoffeeHub.Web`
- a simple authenticated home page that lists coffees from the database
- xUnit tests for `UserService` and `CoffeeService`
- documentation in `README.md` files and `docs/`

## Project Rules

- `CoffeeHub.Domain`: entities and domain concepts only
- `CoffeeHub.Application`: interfaces, services, validation, authentication flow, use case orchestration
- `CoffeeHub.Infrastructure`: EF Core, `DbContext`, mappings, password hashing, repository implementations
- `CoffeeHub.Api`: REST endpoints and HTTP concerns only
- `CoffeeHub.Web`: Razor Pages UI, cookie auth, and presentation concerns only
- `CoffeeHub.Tests`: unit tests for application behavior

## What To Avoid

- Do not place EF Core code inside `Domain`.
- Do not place persistence logic inside controllers or pages.
- Do not place direct database access inside `Application` services.
- Do not use `Api` or `Web` as the place for business rules.
- Do not mix naming conventions or switch between Portuguese and English in code.

## Next Recommended Steps

- improve the home feed with richer coffee information instead of raw IDs
- add pages or flows for creating coffees through the web application
- add review and recipe application services
- expand tests to authentication and repository integration scenarios
- continue documenting real use cases as the product grows
