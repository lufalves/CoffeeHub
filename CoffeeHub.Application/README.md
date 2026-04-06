# CoffeeHub.Application

This project contains the application layer of CoffeeHub.

## Responsibility

`CoffeeHub.Application` coordinates use cases and enforces application-level rules before data reaches infrastructure.

This is the layer between the entry points (`Api`, `Web`) and persistence (`Infrastructure`).

## Current Structure

| Folder | Contents |
|---|---|
| `Abstractions/` | Base classes and shared patterns (e.g., `CrudServiceBase`) |
| `Common/` | Shared utilities (`EntityValidator`, `PagedResult`) |
| `Interfaces/` | Service and repository contracts |
| `Services/` | Application service implementations |

## Services

| Service | Responsibility |
|---|---|
| `AuthService` | User registration, login validation, password hashing, brute-force protection, profile updates |
| `UserService` | User CRUD operations, email uniqueness validation |
| `CoffeeService` | Coffee CRUD operations, barcode uniqueness validation, paginated listing with eager loading |
| `RecipeService` | Recipe read operations (create/edit/delete pending) |
| `ReviewService` | Review read operations (create/edit/delete pending) |
| `RoasteryService` | Roastery CRUD operations |
| `OriginService` | Origin CRUD operations |
| `FarmService` | Farm CRUD operations |
| `BeanVarietyService` | Bean variety CRUD operations |
| `RoastLevelService` | Roast level CRUD operations |
| `BrewingMethodService` | Brewing method CRUD operations |

## Common Abstractions

| Type | Purpose |
|---|---|
| `CrudServiceBase<T>` | Generic base class for CRUD services |
| `EntityValidator` | Validates entity existence and required fields |
| `PagedResult<T>` | Paginated result wrapper with total count |
| `ICrudRepository<T>` | Generic CRUD repository contract |
| `ICrudService<T>` | Generic CRUD service contract |

## What Belongs Here

- service interfaces
- repository interfaces
- authentication interfaces
- application services
- validation and orchestration logic
- use case coordination
- pagination and filtering abstractions

## What Should Not Be Here

- EF Core `DbContext`
- SQL or PostgreSQL code
- controller code
- Razor Pages code
- direct infrastructure implementations

## Current Responsibilities

This layer currently handles rules such as:

- user validation and duplicate email checks
- coffee validation and duplicate barcode checks
- registration flow with automatic Admin role for first user
- credential validation before login
- brute-force login protection (5 attempts, 15-min lockout)
- paginated queries with eager loading for navigation properties
- profile updates (name, email, avatar, password change)

## Intended Flow

`Api/Web -> Application Service -> Repository Interface -> Infrastructure`

## Goal

Keep use cases explicit, testable, and independent from infrastructure details.
