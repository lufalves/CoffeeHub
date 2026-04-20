# CoffeeHub.Tests

This project contains automated tests for CoffeeHub.

## Responsibility

`CoffeeHub.Tests` validates application behavior with fast unit tests and integration tests.

## Current Stack

- xUnit v3 (`3.2.2`)
- `Microsoft.NET.Test.Sdk`
- `xunit.runner.visualstudio` (mandatory for `dotnet test` discovery)
- `xunit.analyzers`
- `coverlet.collector` for code coverage
- `coverlet.runsettings` for coverage configuration

## Current Structure

| File/Folder | Contents |
|---|---|
| `AuthServiceTests.cs` | 12 unit tests for authentication flow (registration, login, input validation) |
| `UserServiceTests.cs` | 11 unit tests for user CRUD, profile, avatar, password change |
| `CoffeeServiceTests.cs` | 8 unit tests for coffee CRUD, barcode uniqueness |
| `RecipeServiceTests.cs` | 12 unit tests for recipe CRUD, title/amount validation, FK guard clauses |
| `ReviewServiceTests.cs` | 11 unit tests for review CRUD, rating range, comment length, FK guard clauses |
| `Builders/` | Test data builders (`TestCoffeeBuilder`, `TestUserBuilder`, `TestRecipeBuilder`, `TestReviewBuilder`, `IntegrationEntityFactory`) |
| `Fakes/` | Centralized fake repository implementations (`FakeAuthUserRepository`, `FakeRecipeRepository`, `FakeReviewRepository`, `FakePasswordHashService`) |
| `Common/` | Test utilities (`ServiceTestBase`, `IntegrationDbFixture`, `SqliteTestDatabase`, `AssemblyInfo`) |

## Current Test Coverage

### AuthService (12 tests)
- First user gets Admin role
- Subsequent users get User role
- Duplicate email rejection
- Invalid name validation (null, empty, whitespace, too long)
- Invalid email validation (null, empty, whitespace, invalid format, too long)
- Invalid password validation (null, empty, whitespace, too short)
- Correct credentials login
- Wrong password login
- Non-existent email login
- Null/empty input validation for login

### UserService (11 tests)
- User creation and validation
- Duplicate email rejection
- Empty name rejection
- Update returns null when user doesn't exist
- Update applies changes when user exists
- Soft delete returns false when user doesn't exist
- Soft delete succeeds when user exists
- Profile update normalizes email
- Avatar clearing
- Password change with valid current password
- Password change rejects invalid current password

### CoffeeService (8 tests)
- Coffee creation and validation
- Empty RoasteryId rejection
- Update returns null when coffee doesn't exist
- Update applies changes when coffee exists
- Soft delete returns false when coffee doesn't exist
- Soft delete succeeds when coffee exists
- Duplicate barcode rejection
- Empty barcode rejection

### RecipeService (12 tests)
- Recipe creation when data is valid
- Throws when UserId is empty
- Throws when CoffeeId is empty
- Throws when BrewingMethodId is empty
- Throws when Title is null or empty (Theory)
- Throws when Title exceeds 200 chars
- Throws when CoffeeAmount is negative or zero (Theory)
- Throws when WaterAmount is negative or zero (Theory)
- Update returns null when recipe does not exist
- Update applies changes when recipe exists
- Soft delete returns false when recipe does not exist
- Soft delete succeeds when recipe exists

### ReviewService (11 tests)
- Review creation when data is valid
- Throws when UserId is empty
- Throws when CoffeeId is empty
- Throws when Rating is below 1 (Theory)
- Throws when Rating is above 5 (Theory)
- Throws when Comment exceeds 2000 chars
- Update returns null when review does not exist
- Update applies changes when review exists
- Update throws when rating is invalid (Theory)
- Soft delete returns false when review does not exist
- Soft delete succeeds when review exists

## Tested Rules

| Rule | Status |
|---|---|
| Duplicate email rejection | ✅ Tested |
| Duplicate coffee barcode rejection | ✅ Tested |
| Required field validation (name, email, password, barcode, title) | ✅ Tested |
| Update flow behavior | ✅ Tested |
| Soft delete behavior | ✅ Tested |
| Profile updates (name, email, avatar) | ✅ Tested |
| Password change | ✅ Tested |
| First user = Admin role assignment | ✅ Tested |
| Recipe title length limit (200 chars) | ✅ Tested |
| Recipe amount positivity validation | ✅ Tested |
| Review rating range (1-5) | ✅ Tested |
| Review comment length limit (2000 chars) | ✅ Tested |
| FK guard clauses (empty Guid) | ✅ Tested |

## Known Gaps (TODO)

| Rule / Service | Status | Notes |
|---|---|---|
| Brute-force login protection | ⚪ N/A | `LoginAttemptTracker` is Web-only (`CoffeeHub.Web`). Not an Application-layer concern |
| Paginated listing (CoffeeService) | ❌ Not tested | `GetPagedAsync` not exercised |
| Integration tests (repository persistence) | ❌ Not tested | `IntegrationDbFixture` infrastructure is ready but no test class exists |

## How to Run Tests

```bash
# Run all tests
dotnet test

# Run with code coverage
dotnet test --settings coverlet.runsettings

# Run specific test class
dotnet test --filter "FullyQualifiedName~RecipeServiceTests"
```

## Goal

Keep the most important application rules protected while the project is still evolving quickly. Target minimum 80% coverage for Application services.
