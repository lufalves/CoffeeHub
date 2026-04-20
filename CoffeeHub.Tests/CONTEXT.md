# CoffeeHub.Tests - Technical Context

**Dependencies:** `CoffeeHub.Application`, `CoffeeHub.Domain`, `CoffeeHub.Infrastructure`  
**Root Reference:** [Global Context](../CONTEXT.md)

## Mission
Unified test stack wrapping continuous assertions base for the `Application` flow down into solid `Infrastructure` connections.

## Core Tooling
- Engine based on **`xUnit v3`** (`3.2.2`). Standard C# testing via `Microsoft.NET.Test.Sdk` accompanied by the mandatory `xunit.runner.visualstudio` for optimal `dotnet test` discovery.
- **Strict Async Safety (`xUnit1051`)**: Test functions utilizing asynchronous methods must explicitly bypass `default` parameter omissions by passing `TestContext.Current.CancellationToken` to avoid deadlocking the test runner blockings.
- Coverage engine mapped natively by `coverlet.collector` with manual min-target percent setup on runsettings (Targeting typically an 80% baseline).

## Internal Structure Patterns
- **`/Fakes`**: Mimic classes injected to encapsulate pure dependencies (`FakeAuthUserRepository`, `FakeRecipeRepository`, `FakeReviewRepository`, `FakePasswordHashService`). No inline Fakes in test files — all centralized here.
- **`/Builders`**: Classic Builder Pattern approach (`TestCoffeeBuilder`, `TestUserBuilder`, `TestRecipeBuilder`, `TestReviewBuilder`) + `IntegrationEntityFactory` for reference entities.
- **`/Common`**: `ServiceTestBase` (NullLogger helper), `IntegrationDbFixture` (SQLite in-memory), `AssemblyInfo` (parallelism config).

## Current Test File Layout
Test files are located at the **project root** (not in subfolders):
- `AuthServiceTests.cs` (12 tests) — Registration, login validation, input guard clauses
- `UserServiceTests.cs` (11 tests) — CRUD, profile update, avatar, password change, soft-delete
- `CoffeeServiceTests.cs` (8 tests) — CRUD, barcode uniqueness, soft-delete
- `RecipeServiceTests.cs` (12 tests) — Full CRUD, title/amount validations, FK guard clauses
- `ReviewServiceTests.cs` (11 tests) — Full CRUD, rating range, comment length, FK guard clauses

## Consistency Rules
- All test classes MUST inherit `ServiceTestBase` and use its `Logger<T>()` helper.
- No inline Fake classes allowed in test files — use centralized `/Fakes/FakeRepositories.cs`.
- All async test calls must pass `TestContext.Current.CancellationToken` explicitly.

## Remaining Gaps (TODO)
- **AuthService brute-force lockout**: `LoginAttemptTracker` logic is **Web-only** (`CoffeeHub.Web`). No Application-layer unit test is applicable.
- **Integration tests**: `IntegrationDbFixture` + `SqliteTestDatabase` infrastructure is ready but **no integration test class exists yet**.
- **Paginated listing (CoffeeService)**: `GetPagedAsync` not exercised.
