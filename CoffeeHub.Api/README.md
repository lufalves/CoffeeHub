# CoffeeHub.Api

This project exposes the REST API of CoffeeHub.

## Responsibility

`CoffeeHub.Api` is the HTTP entry point for external clients such as future mobile apps, integrations, and other consumers.

## What Belongs Here

- controllers
- HTTP routing
- request and response handling
- status code decisions
- API-specific configuration

## What Should Not Be Here

- database access logic
- EF Core mappings
- business rules
- domain orchestration

## Expected Flow

Controllers should remain thin.

Preferred flow:

`Controller -> Application Service -> Repository -> Database`

## Current Endpoints

The API currently exposes initial CRUD endpoints for:

- `Users`
- `Coffees`

Current API behavior already includes:

- `400 Bad Request` for invalid input
- `409 Conflict` for duplicate business constraints where applicable

## Current Notes

- coffee creation now depends on a required unique barcode
- the API is still intentionally simple and uses domain entities directly
- DTO separation is still a future improvement

## Goal

Keep the API focused on transport concerns and delegate application logic to `CoffeeHub.Application`.
