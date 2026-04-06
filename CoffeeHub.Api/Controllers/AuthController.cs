using CoffeeHub.Api.Authentication;
using CoffeeHub.Api.Configuration;
using CoffeeHub.Api.Contracts.Requests;
using CoffeeHub.Api.Contracts.Responses;
using CoffeeHub.Api.Mapping;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;

namespace CoffeeHub.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(
    IAuthService authService,
    IUserService userService,
    IJwtTokenService jwtTokenService,
    IRefreshTokenStore refreshTokenStore,
    IOptions<JwtOptions> jwtOptions) : ControllerBase
{
    /// <summary>
    /// Authenticates a user and returns JWT and refresh tokens.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login")]
    [EnableRateLimiting("login")]
    [ProducesResponseType(typeof(AuthTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthTokenResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await authService.ValidateCredentialsAsync(request.Email, request.Password, cancellationToken);

        if (user is null)
        {
            return Unauthorized(new ErrorResponse(new ErrorDetail("auth.invalid_credentials", "Email or password is invalid.")));
        }

        var response = await BuildAuthResponseAsync(user, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Registers a new user and returns JWT and refresh tokens.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthTokenResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<AuthTokenResponse>> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var user = await authService.RegisterAsync(request.Name, request.Email, request.Password, request.AvatarUrl, cancellationToken);
        var response = await BuildAuthResponseAsync(user, cancellationToken);

        return Created($"/api/v1/users/{user.Id}", response);
    }

    /// <summary>
    /// Exchanges a refresh token for a new access token.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthTokenResponse>> Refresh(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var userId = await refreshTokenStore.ConsumeAsync(request.RefreshToken, cancellationToken);

        if (!userId.HasValue)
        {
            return Unauthorized(new ErrorResponse(new ErrorDetail("auth.invalid_refresh_token", "Refresh token is invalid or expired.")));
        }

        var user = await userService.GetByIdAsync(userId.Value, cancellationToken);

        if (user is null)
        {
            return Unauthorized(new ErrorResponse(new ErrorDetail("auth.invalid_refresh_token", "Refresh token is invalid or expired.")));
        }

        var response = await BuildAuthResponseAsync(user, cancellationToken);
        return Ok(response);
    }

    private async Task<AuthTokenResponse> BuildAuthResponseAsync(User user, CancellationToken cancellationToken)
    {
        var (accessToken, accessTokenExpiresAt) = jwtTokenService.GenerateAccessToken(user);
        var refreshToken = jwtTokenService.GenerateRefreshToken();
        var refreshTokenExpiresAt = DateTimeOffset.UtcNow.AddDays(jwtOptions.Value.RefreshTokenDays);

        await refreshTokenStore.StoreAsync(refreshToken, user.Id, refreshTokenExpiresAt, cancellationToken);

        return new AuthTokenResponse(
            accessToken,
            accessTokenExpiresAt,
            refreshToken,
            refreshTokenExpiresAt,
            user.ToResponse());
    }
}
