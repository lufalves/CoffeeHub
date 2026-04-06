# CoffeeHub.Tests

This project contains automated tests for CoffeeHub.

## Responsibility

`CoffeeHub.Tests` validates application behavior with fast unit tests and integration tests.

## Current Stack

- xUnit
- `Microsoft.NET.Test.Sdk`
- `coverlet.collector` for code coverage
- `coverlet.runsettings` for coverage configuration

## Current Structure

| File/Folder | Contents |
|---|---|
| `AuthServiceTests.cs` | Unit tests for authentication flow |
| `UserServiceTests.cs` | Unit tests for user CRUD operations |
| `CoffeeServiceTests.cs` | Unit tests for coffee CRUD operations |
| `Builders/` | Test data builders (`TestEntityBuilders.cs`) |
| `Fakes/` | Fake repository implementations (`FakeRepositories.cs`) |
| `Common/` | Test utilities (`ServiceTestBase`, `IntegrationDbFixture`, `AssemblyInfo`) |
| `Integration/` | Integration tests against PostgreSQL |

## Current Test Coverage

### AuthService
- User registration flow
- Login validation
- Brute-force protection (lockout after 5 attempts)
- Password validation
- Profile updates

### UserService
- User creation and validation
- Duplicate email rejection
- Required field validation
- Update flow behavior
- Soft delete behavior

### CoffeeService
- Coffee creation and validation
- Duplicate barcode rejection
- Required field validation
- Paginated listing
- Update flow behavior
- Soft delete behavior

## Tested Rules

| Rule | Status |
|---|---|
| Duplicate email rejection | ✅ Tested |
| Duplicate coffee barcode rejection | ✅ Tested |
| Required field validation | ✅ Tested |
| Update flow behavior | ✅ Tested |
| Soft delete behavior | ✅ Tested |
| Brute-force login protection | ✅ Tested |
| Profile updates | ✅ Tested |

## How to Run Tests

```bash
# Run all tests
dotnet test

# Run with code coverage
dotnet test --settings coverlet.runsettings

# Run specific test class
dotnet test --filter "FullyQualifiedName~UserServiceTests"
```

## Goal

Keep the most important application rules protected while the project is still evolving quickly. Target minimum 80% coverage for Application services.
