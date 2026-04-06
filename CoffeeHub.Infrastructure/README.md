# CoffeeHub.Infrastructure

This project contains the infrastructure layer of CoffeeHub.

## Responsibility

`CoffeeHub.Infrastructure` is responsible for technical implementations used by the application.

Its main focus currently is:

- EF Core with PostgreSQL
- repository implementations
- password hashing
- dependency injection registration
- database migrations and configurations

## Current Structure

| Folder | Contents |
|---|---|
| `Persistence/` | `CoffeeHubDbContext`, EF Core configurations, migrations |
| `Persistence/Configurations/` | Entity-specific EF Core configurations with indexes and constraints |
| `Persistence/Migrations/` | EF Core migration files |
| `Repositories/` | Implementations of application repository interfaces |
| `Services/` | Infrastructure services (password hashing via ASP.NET Core PasswordHasher) |
| `Common/` | `CrudRepositoryBase` generic base class |
| `Data/` | Seed data for reference tables |
| `DependencyInjection.cs` | Service registration helpers for DI container |

## Repositories

| Repository | Entity |
|---|---|
| `UserRepository` | User |
| `CoffeeRepository` | Coffee (with eager loading for 5 navigation properties) |
| `RecipeRepository` | Recipe |
| `ReviewRepository` | Review |
| `RoasteryRepository` | Roastery |
| `OriginRepository` | Origin |
| `FarmRepository` | Farm |
| `BeanVarietyRepository` | BeanVariety |
| `RoastLevelRepository` | RoastLevel |
| `BrewingMethodRepository` | BrewingMethod |
| `CoffeeShopRepository` | CoffeeShop |

## What Belongs Here

- `CoffeeHubDbContext`
- EF Core entity configurations
- migrations
- repository implementations
- provider-specific persistence code
- hashing implementation
- infrastructure registrations
- seed data

## What Should Not Be Here

- controller logic
- Razor Pages logic
- business rules that belong to the application layer
- domain concepts unrelated to technical implementation

## Current Notes

- PostgreSQL is the current database provider
- EF Core migrations are in use
- `Coffee.Barcode` is enforced as a unique database index
- Eager loading configured for Coffee (Roastery, Origin, BeanVariety, RoastLevel, Farm)
- Soft delete filter applied globally via EF Core query filters
- Indexes on frequently queried fields

## Dependency Rule

`Infrastructure` can depend on:

- `Application`
- `Domain`

But `Application` should not depend on infrastructure implementations.

## Goal

Keep all technical persistence concerns isolated here so the rest of the solution remains clean and easier to evolve.
