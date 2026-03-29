# CoffeeHub.Tests

This project contains automated tests for CoffeeHub.

## Responsibility

`CoffeeHub.Tests` validates application behavior with fast unit tests.

## Current Stack

- xUnit
- `Microsoft.NET.Test.Sdk`
- `coverlet.collector`

## Current Scope

The test project currently covers:

- `UserService`
- `CoffeeService`

Current tested rules include:

- duplicate email rejection
- duplicate coffee barcode rejection
- required-field validation
- update flow behavior
- soft delete behavior

## Goal

Keep the most important application rules protected while the project is still evolving quickly.
