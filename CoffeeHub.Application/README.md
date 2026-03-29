# CoffeeHub.Application

This project contains the application layer of CoffeeHub.

## Responsibility

`CoffeeHub.Application` coordinates use cases and enforces application-level rules before data reaches infrastructure.

This is the layer between the entry points (`Api`, `Web`) and persistence (`Infrastructure`).

## Current Structure

- `Interfaces`: contracts used by the application layer
- `Services`: application services that implement use cases and validations

## What Belongs Here

- service interfaces
- repository interfaces
- authentication interfaces
- application services
- validation and orchestration logic
- use case coordination

## What Should Not Be Here

- EF Core `DbContext`
- SQL or PostgreSQL code
- controller code
- Razor Pages code
- direct infrastructure implementations

## Current Services

Current application services include:

- `UserService`
- `CoffeeService`
- `AuthService`

## Current Responsibilities

This layer currently handles rules such as:

- user validation
- coffee validation
- duplicate email checks
- duplicate coffee barcode checks
- registration flow
- credential validation before login

## Intended Flow

`Api/Web -> Application Service -> Repository Interface -> Infrastructure`

## Goal

Keep use cases explicit, testable, and independent from infrastructure details.
