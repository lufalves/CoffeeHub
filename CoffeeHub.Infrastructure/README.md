# CoffeeHub.Infrastructure

This project contains the infrastructure layer of CoffeeHub.

## Responsibility

`CoffeeHub.Infrastructure` is responsible for technical implementations used by the application.

Its main focus currently is:

- EF Core
- PostgreSQL
- repository implementations
- password hashing
- dependency injection registration

## Current Structure

- `Persistence`: `DbContext`, design-time factory, EF Core mappings, migrations
- `Repositories`: implementations of application repository interfaces
- `Services`: infrastructure services such as password hashing
- `DependencyInjection.cs`: registration helpers for DI

## What Belongs Here

- `CoffeeHubDbContext`
- EF Core entity configurations
- migrations
- repository implementations
- provider-specific persistence code
- hashing implementation
- infrastructure registrations

## What Should Not Be Here

- controller logic
- Razor Pages logic
- business rules that belong to the application layer
- domain concepts unrelated to technical implementation

## Current Notes

- PostgreSQL is the current database provider
- EF Core migrations are already in use
- `Coffee.Barcode` is enforced as a unique database index

## Dependency Rule

`Infrastructure` can depend on:

- `Application`
- `Domain`

But `Application` should not depend on infrastructure implementations.

## Goal

Keep all technical persistence concerns isolated here so the rest of the solution remains clean and easier to evolve.
