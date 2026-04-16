# CoffeeHub.Tests - Technical Context

**Dependencies:** Clean overall solution  
**Root Reference:** [Global Context](../CONTEXT.md)

## Mission
Unified test stack wrapping continuous assertions base for the `Application` flow down into solid `Infrastructure` connections.

## Core Tooling
- Engine based on **`xUnit v3`**. Standard C# testing via `Microsoft.NET.Test.Sdk` accompanied by the mandatory `xunit.runner.visualstudio` for optimal `dotnet test` discovery.
- **Strict Async Safety (`xUnit1051`)**: Test functions utilizing asynchronous methods must explicitly bypass `default` parameter omissions by passing `TestContext.Current.CancellationToken` to avoid deadlocking the test runner blockings.
- Coverage engine mapped natively by `coverlet.collector` with manual min-target percent setup on runsettings (Targeting typically an 80% baseline).

## Internal Structure Patterns
- **`/Fakes`**: Mimic classes injected to encapsulate pure dependencies. Circumvents latencies or complex networking inside unnecessary environments for isolated testing targeting fake `FakeRepositories.cs`.
- **`/Builders`**: Classic Builder Pattern approach (`TestEntityBuilders.cs`) speeds generic injections for large entities and eases instantiating rich dependent classes into the pipeline without generating redundant bloat.

## Scopes and Targeting
- **Unit Tests (`/UnitTests`):** Directed dense data targeted cleanly evaluating exclusively the `Application` layer. Specific tests (`AuthServiceTests.cs`, `UserServiceTests.cs`, `CoffeeServiceTests.cs`) test precision and outcomes upon:
  - Login Brute Force limits bypassed.
  - Record injection (Ex Emails and Barcodes) cloning logic (verifying uniqueness blockers).
  - Isolated global coupled rules like soft-delete filtering.

- **Integration Tests (`/Integration`):** Framework base referencing `IntegrationDbFixture` classes to boot either a real base or postgres container mapping active connection to evaluate cross-saving to the Database.
