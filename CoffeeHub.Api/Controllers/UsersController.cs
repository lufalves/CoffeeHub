using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.User;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<User>>> GetAll(CancellationToken cancellationToken)
    {
        var users = await userService.GetAllAsync(cancellationToken);
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<User>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var user = await userService.GetByIdAsync(id, cancellationToken);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> Create(CreateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = request.PasswordHash,
                AvatarUrl = request.AvatarUrl
            };

            var createdUser = await userService.CreateAsync(user, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new { message = exception.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<User>> Update(Guid id, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var updatedUser = await userService.UpdateAsync(
                new User
                {
                    Id = id,
                    Name = request.Name,
                    Email = request.Email,
                    PasswordHash = request.PasswordHash,
                    AvatarUrl = request.AvatarUrl
                },
                cancellationToken);

            return updatedUser is null ? NotFound() : Ok(updatedUser);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new { message = exception.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await userService.SoftDeleteAsync(id, cancellationToken);
            return deleted ? NoContent() : NotFound();
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }
}

public record CreateUserRequest(
    string Name,
    string Email,
    string PasswordHash,
    string? AvatarUrl);

public record UpdateUserRequest(
    string Name,
    string Email,
    string PasswordHash,
    string? AvatarUrl);
