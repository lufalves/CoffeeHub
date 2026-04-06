# CoffeeHub.Domain

This project contains the core domain model of CoffeeHub.

## Responsibility

`CoffeeHub.Domain` represents the business concepts and rules of the application.

## Current Entities

| Entity | Description |
|---|---|
| `User` | Platform users with roles (Admin/User), email, avatar, and login failure tracking |
| `Coffee` | Coffee catalog entries with barcode, roastery, origin, bean variety, and roast level |
| `Review` | User reviews with ratings and notes for coffees |
| `Recipe` | Brewing recipes with instructions, method, and parameters |
| `Roastery` | Coffee roastery reference data with name and country |
| `CoffeeShop` | Coffee shop reference data |
| `Origin` | Coffee origin country/region |
| `Farm` | Coffee farm linked to an origin |
| `BeanVariety` | Coffee bean variety reference data |
| `RoastLevel` | Roast level reference data (name, description, temperature range) |
| `BrewingMethod` | Brewing method reference data |

## Value Objects

| Value Object | Purpose |
|---|---|
| `BarcodeValue` | Encapsulates barcode format validation |
| `EmailAddress` | Encapsulates email format validation |
| `UrlValue` | Encapsulates URL format validation |

## Common Types

| Type | Purpose |
|---|---|
| `EntityBase` | Abstract base class with Id, audit fields, and soft delete support |
| `IEntity` | Marker interface for domain entities |

## What Belongs Here

- entities
- value objects
- enums
- domain-specific concepts
- domain rules that truly belong to the model itself

## What Should Not Be Here

- EF Core configuration
- database access
- HTTP logic
- controller logic
- Razor Pages logic
- infrastructure dependencies

## Current Conventions

- entities use singular names
- all entities inherit from `EntityBase`
- IDs use `Guid`
- audit fields use `DateTimeOffset` (`CreatedAt`, `UpdatedAt`)
- soft delete via `IsDeleted` flag
- code is written in English

## Important Current Rules

- `Coffee` requires a `Barcode`
- `Coffee.Barcode` must be unique (enforced by application and database layers)
- First registered user is automatically assigned Admin role
- Users track login failures and lockout expiration for brute-force protection

## Dependency Rule

`CoffeeHub.Domain` should be the most independent project in the solution.

It should not depend on:

- `Application`
- `Infrastructure`
- `Api`
- `Web`
- `Tests`

## Goal

Keep the domain simple, expressive, and stable. It should describe the business, not the technical implementation.
