# CoffeeHub.Api

This project exposes the REST API of CoffeeHub.

## Responsibility

`CoffeeHub.Api` is the HTTP entry point for external clients such as future mobile apps, integrations, and other consumers.

## Current Structure

| Folder | Contents |
|---|---|
| `Controllers/` | REST API controllers for all entities |
| `Authentication/` | JWT Bearer authentication configuration and handlers |
| `Contracts/` | Request and response DTOs |
| `Mapping/` | DTO-to-entity mapping utilities |
| `Middleware/` | Custom middleware (error handling, request logging) |
| `Configuration/` | API-specific configuration classes |

## Controllers

| Controller | Endpoints |
|---|---|
| `AuthController` | Login, Register, Token Refresh |
| `UsersController` | User CRUD operations |
| `CoffeesController` | Coffee CRUD operations with pagination |
| `RecipesController` | Recipe CRUD operations |
| `ReviewsController` | Review CRUD operations |
| `RoasteriesController` | Roastery CRUD operations |
| `OriginsController` | Origin CRUD operations |
| `FarmsController` | Farm CRUD operations |
| `BeanVarietiesController` | Bean variety CRUD operations |
| `RoastLevelsController` | Roast level CRUD operations |
| `BrewingMethodsController` | Brewing method CRUD operations |

## Authentication

- JWT Bearer authentication scheme
- Token-based authentication for API consumers
- `[Authorize]` attribute on protected endpoints
- Role-based authorization (Admin/User)
- Token refresh endpoint for session continuity

## What Belongs Here

- controllers
- HTTP routing
- request and response handling
- status code decisions
- API-specific configuration
- JWT authentication setup
- request/response DTOs
- DTO-to-entity mapping

## What Should Not Be Here

- database access logic
- EF Core mappings
- business rules
- domain orchestration

## Expected Flow

Controllers should remain thin.

Preferred flow:

`Controller -> Application Service -> Repository -> Database`

## Current Notes

- All CRUD endpoints are available for all entities
- JWT Bearer authentication is applied to all endpoints
- Request/Response DTOs are used to avoid exposing domain entities
- `400 Bad Request` for invalid input
- `409 Conflict` for duplicate business constraints
- Pagination supported on listing endpoints

## Goal

Keep the API focused on transport concerns and delegate application logic to `CoffeeHub.Application`.
