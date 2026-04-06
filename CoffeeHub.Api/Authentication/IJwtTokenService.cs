using CoffeeHub.Domain.User;

namespace CoffeeHub.Api.Authentication;

public interface IJwtTokenService
{
    (string Token, DateTimeOffset ExpiresAt) GenerateAccessToken(User user);
    string GenerateRefreshToken();
}
