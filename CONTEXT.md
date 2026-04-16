# CoffeeHub - Global Technical Context

This is the root architectural map (Onion/Clean Architecture). For focused details, browse the contexts of each individual project.

## Projects and Layers

| Project | Layer | Detailed Context |
|---------|--------|--------------------|
| **CoffeeHub.Domain** | Core/Domain | [View](./CoffeeHub.Domain/CONTEXT.md) |
| **CoffeeHub.Application** | Use Cases/Services | [View](./CoffeeHub.Application/CONTEXT.md) |
| **CoffeeHub.Infrastructure** | Data/EF Core/Auth | [View](./CoffeeHub.Infrastructure/CONTEXT.md) |
| **CoffeeHub.Api** | REST/JWT | [View](./CoffeeHub.Api/CONTEXT.md) |
| **CoffeeHub.Web** | UI/Razor Pages/Cookie Auth | [View](./CoffeeHub.Web/CONTEXT.md) |
| **CoffeeHub.Tests** | Unit/Integration (xUnit) | [View](./CoffeeHub.Tests/CONTEXT.md) |

## Authentication and Authorization Flow

- **API (`CoffeeHub.Api`)**: `Controller -> AuthService -> JWT Bearer Token`
- **Web (`CoffeeHub.Web`)**: `Razor Page -> AuthService -> IUserRepository -> PostgreSQL -> Cookie Authentication`
- **Rules:** The first user registered in the system automatically receives the Admin role. Brute-force lockout is enabled (5 attempts / 15-min lockout). Password hashing is performed using the ASP.NET `PasswordHasher`.

## Architecture and Standard Flow

- **Data Access**: `Api/Web -> Application Services (ICrudService<T>) -> Repository Interfaces (ICrudRepository<T>) -> EF Core (CoffeeHubDbContext) -> PostgreSQL`
- **IDs**: Internally typed as `Guid`.
- **Deletion**: Global soft-delete (`IsDeleted` property) fully supported by `EntityBase`.
- **Entity State**: Audit fields (`CreatedAt`, `UpdatedAt`) are encapsulated in the base abstraction which spans across the domain.
