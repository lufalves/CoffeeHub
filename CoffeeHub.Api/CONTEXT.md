# CoffeeHub.Api - Technical Context

**Dependencies:** `CoffeeHub.Application`, `CoffeeHub.Infrastructure`  
**Root Reference:** [Global Context](../CONTEXT.md)

## Mission
Entry point via pure RESTful API HTTP exposure. Isolated for third parties, mobile apps, etc.

## Setup and Core
- Routes `Controllers` mapping pure base HTTP methods (GET, POST, PUT, DELETE).
- **JWT Middleware**: Guarded authentication using `Bearer` scheme. Requires a valid token. Annotations via `[Authorize]` handle role-based access (Admin/User). Time management relies organically on the .NET 8 `TimeProvider` standard interface, strictly deprecating manual `ISystemClock` injections.
- **Global Error Handling (`Middleware/`)**: Captures exceptions directly and logs requests in internal global handlers, shielding stack traces and 5xx errors from leaking to the public web.

## Featured Endpoints (`/Controllers`)
- **`AuthController`**: Token handshake (Login, new Refresh/Access Token generation, and raw Registration routes).
- Generic CRUD through `CoffeesController`, `RecipesController`, etc. to act as object roundabouts.
- Constraints trigger `400 Bad Request` through contract validations. Business rule errors (such as duplication) trigger `409 Conflict` when hitting DB/Application constraints.
- Paginations are implemented with `offset` logic on heavy GET routes.

## Contracts (`/Contracts`) and Data Flow (`/Mapping`)
- Output and consumption are strictly guarded using the DTO pattern (Request Models, Response Models). Exposes Entities encapsulating only authorized fields.
- The `Mapping` process routes the DTO back to a native Entity class which internally navigates the _Application_ requests, leaving only via the endpoint's return route.
