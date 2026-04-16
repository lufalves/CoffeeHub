# CoffeeHub.Infrastructure - Technical Context

**Dependencies:** `CoffeeHub.Application`, `CoffeeHub.Domain`  
**Root Reference:** [Global Context](../CONTEXT.md)

## Mission
Technical dependencies provider. Specially responsible for the real database communication using **EF Core** configured for **PostgreSQL**.

## Core and Context (EF)
- **`CoffeeHubDbContext`**: The root `DbContext` centralizing DbSets and ModelBuilding. Injected as Scoped internally.
- **EF Configurations (`Persistence/Configurations/`)**: Encompasses strongly typed mappings. Notable rules applied here: Index guaranter that prevents duplicates (Ex: Uniqueness on `Coffee` Barcode).
- Soft-Delete: Transparent query filters configured natively in the model (`x => !x.IsDeleted`).
- **Seed Data**: Automatic population and import at initializations for passive domain support tables via migrations or manual init in the persistent layer.

## Repositories (`/Repositories`)
- **`CrudRepositoryBase`**: Wraps the basic verbs on top of EF mapped to the `.Application` contracts.
- `UserRepository`, `CoffeeRepository` implement these direct linkages.
- **Eager Loading**: The `Coffee` repository implements string-based `.Include()` natively in fetching logic to extract complex relationships (Origin, Roastery, Beans and equivalents) into the request.

## Hash and SecTools
- Native authentication hash delegator pointing to the main engine `PasswordHasher` of ASP.NET Core natively.

## DependencyInjection.cs
- Aggregator point for `IServiceCollection`. Couples the connection string, repositories in explicit format for Scoped injection, and the database Context itself into the base caller pipeline layer.
