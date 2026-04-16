# CoffeeHub.Application - Technical Context

**Dependencies:** `CoffeeHub.Domain`  
**Root Reference:** [Global Context](../CONTEXT.md)

## Mission
Orchestrate Business Rules and Use Cases through Services. Receives requests from clients (Web or Api), validates cross-logic, routes to DB interfaces, and returns processed data without polluting UI with internal complexity.

## Abstractions and Models (DI)
- **`ICrudRepository<T>`:** Generic interface, points to infrastructure.
- **`ICrudService<T>`:** Generic service routing rules and base DB logic.
- **`CrudServiceBase<T>`:** Base class implementing the standard repetition (`Create`, `Update`, and `Soft Delete` with base auditing).
- **`EntityValidator`**: Helper utility to validate if FKs/Data are coherent.
- **`PagedResult<T>`:** Generic wrapper responsible for routing `TotalCount` together with lists in limited page requests (Offset pagination).

## Orchestrator Services (`/Services`)
- `AuthService`: Engine targeting Registration, Login SignIn, Brute-Force Defenses (15-min block after repeated failure), Hash Validation, visual profile edits (User/Email/Avatar).
- `CoffeeService`: Manages `Coffee`, validates and aborts duplicate entries seen via `Barcode`. Triggers Eager Load injections to list nested entities and wide details.
- CRUD patterns repetition for passive structures: `User`, `Recipe`, `Review`, `Roastery`, `Origin`, `Farm`, `BeanVariety`, `RoastLevel`, `BrewingMethod`.

## Standard Restrictions
- Strictly no explicit use of framework Providers/Databases (Like instantiating `DbContext`). Isolation is kept by depending entirely on injected interfaces.
