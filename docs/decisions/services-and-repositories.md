# Services And Repositories

## Decision

CoffeeHub separates application services from repository interfaces.

## Why

Repositories and services solve different problems.

- repositories deal with persistence
- services deal with application rules and orchestration

## Intended Flow

`Controller/PageModel -> Service -> Repository -> DbContext`

## Benefits

- controllers stay thin
- Razor Page models stay thin
- validation happens before persistence
- infrastructure remains isolated
- application rules become easier to test

## Current Examples

This separation is already visible in:

- `UserService`
- `CoffeeService`
- `AuthService`

For example:

- duplicate email validation belongs to services
- duplicate coffee barcode validation belongs to services
- database uniqueness enforcement belongs to repositories and mappings

## Notes

This differs from older patterns where one service layer absorbs both orchestration and persistence behavior. For CoffeeHub, the separation is intentional because the project is also being used to practice cleaner architecture.
