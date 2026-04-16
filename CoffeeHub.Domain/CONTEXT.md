# CoffeeHub.Domain - Technical Context

**Dependencies:** None. Isolated Core.  
**Root Reference:** [Global Context](../CONTEXT.md)

## Entities (Core)
- All inherit from `EntityBase` (`Id: Guid`, `CreatedAt`, `UpdatedAt`, `IsDeleted`).
- **Key Entities:**
  - `User`: Manages Admin/User Roles. Controls failed attempt locks and lockout timeout expirations.
  - `Coffee`: Fundamentally tied to the `Barcode` string and enforces global uniqueness in the database.
  - `Review`: Rating/notes linked to the Coffee Entity.
  - `Recipe`: Brewing methodology linked directly to Coffee preparations.
  - `Roastery`, `Origin`, `Farm`, `BeanVariety`, `RoastLevel`, `BrewingMethod`, `CoffeeShop`: Standalone registers and reference tables associated with the main flows.

## Value Objects
- `BarcodeValue`: Encapsulates pre-validation for identifier formats (EAN/UPC).
- `EmailAddress`: Encapsulates and guards format validation for E-Mail addresses.
- `UrlValue`: URL format validation encapsulation.

## Enforced Rules on this Scope
- Pure passive C# domain. No dependency on injection libraries or EF Core.
- Rich entities. Tightly controlled constructors for restricted setup. Property modification acts only through documented entity behaviors.
