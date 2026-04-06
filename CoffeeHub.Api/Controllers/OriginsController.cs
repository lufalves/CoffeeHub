using CoffeeHub.Api.Contracts.Requests;
using CoffeeHub.Api.Contracts.Responses;
using CoffeeHub.Api.Mapping;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Origin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeHub.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class OriginsController(IOriginService originService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<OriginResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<OriginResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var origins = await originService.GetAllAsync(cancellationToken);
        return Ok(origins.Select(item => item.ToResponse()).ToList());
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OriginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OriginResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var origin = await originService.GetByIdAsync(id, cancellationToken);
        return origin is null ? NotFound() : Ok(origin.ToResponse());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(OriginResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<OriginResponse>> Create(CreateOriginRequest request, CancellationToken cancellationToken)
    {
        var origin = new Origin
        {
            Country = request.Country,
            Region = request.Region,
            Locality = request.Locality,
            Description = request.Description
        };

        var created = await originService.CreateAsync(origin, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.ToResponse());
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(OriginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OriginResponse>> Update(Guid id, UpdateOriginRequest request, CancellationToken cancellationToken)
    {
        var updated = await originService.UpdateAsync(
            new Origin
            {
                Id = id,
                Country = request.Country,
                Region = request.Region,
                Locality = request.Locality,
                Description = request.Description
            },
            cancellationToken);

        return updated is null ? NotFound() : Ok(updated.ToResponse());
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await originService.SoftDeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
