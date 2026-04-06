namespace CoffeeHub.Api.Contracts.Responses;

public sealed record PagedResponse<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalCount, int TotalPages);

public sealed record ErrorResponse(ErrorDetail Error);

public sealed record ErrorDetail(string Code, string Message, object? Details = null);

public sealed record CoffeeResponse(
    Guid Id,
    string Barcode,
    string Name,
    string? Description,
    Guid RoasteryId,
    string? RoasteryName,
    Guid? OriginId,
    string? OriginCountry,
    Guid? FarmId,
    string? FarmName,
    Guid? BeanVarietyId,
    string? BeanVarietyName,
    Guid? RoastLevelId,
    string? RoastLevelName,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

public sealed record UserResponse(
    Guid Id,
    string Name,
    string Email,
    string? AvatarUrl,
    string Role,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

public sealed record RecipeResponse(
    Guid Id,
    Guid UserId,
    Guid CoffeeId,
    string? CoffeeName,
    Guid BrewingMethodId,
    string? BrewingMethodName,
    string Title,
    string? Description,
    decimal? CoffeeAmountInGrams,
    decimal? WaterAmountInMilliliters,
    int? BrewTimeInSeconds,
    string? Instructions,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

public sealed record ReviewResponse(
    Guid Id,
    Guid UserId,
    string? UserName,
    Guid CoffeeId,
    string? CoffeeName,
    decimal Rating,
    string? Comment,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

public sealed record RoasteryResponse(
    Guid Id,
    string Name,
    string? Description,
    string? WebsiteUrl,
    string? InstagramUrl,
    string? LogoUrl,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

public sealed record OriginResponse(
    Guid Id,
    string Country,
    string? Region,
    string? Locality,
    string? Description,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

public sealed record FarmResponse(
    Guid Id,
    string Name,
    Guid? OriginId,
    string? OriginCountry,
    string? ProducerName,
    string? Description,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

public sealed record BeanVarietyResponse(
    Guid Id,
    string Name,
    string? Description,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

public sealed record RoastLevelResponse(
    Guid Id,
    string Name,
    string? Description,
    int DisplayOrder,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

public sealed record BrewingMethodResponse(
    Guid Id,
    string Name,
    string? Description,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

public sealed record AuthTokenResponse(
    string AccessToken,
    DateTimeOffset AccessTokenExpiresAt,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpiresAt,
    UserResponse User);
