# CoffeeHub.Domain

This project contains the core domain model of CoffeeHub.

## Responsibility

`CoffeeHub.Domain` represents the business concepts of the application.

Examples:

- `User`
- `Coffee`
- `Review`
- `Recipe`
- `Roastery`
- `Origin`

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
- IDs use `Guid`
- audit fields use `DateTimeOffset`
- soft delete fields are part of the model where needed
- code is written in English

## Important Current Rules

- `Coffee` requires a `Barcode`
- `Coffee.Barcode` is part of the identity of a catalog entry
- barcode uniqueness is enforced outside the domain by the application and database layers

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
