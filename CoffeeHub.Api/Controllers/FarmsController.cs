using CoffeeHub.Api.Contracts.Requests;
using CoffeeHub.Api.Contracts.Responses;
using CoffeeHub.Api.Mapping;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Farm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeHub.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class FarmsController(IFarmService farmService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<FarmResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<FarmResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var farms = await farmService.GetAllAsync(cancellationToken);
        return Ok(farms.Select(item => item.ToResponse()).ToList());
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(FarmResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FarmResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var farm = await farmService.GetByIdAsync(id, cancellationToken);
        return farm is null ? NotFound() : Ok(farm.ToResponse());
    }

    [HttpGet("origin/{originId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<FarmResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<FarmResponse>>> GetByOrigin(Guid originId, CancellationToken cancellationToken)
    {
        var farms = await farmService.GetByOriginIdAsync(originId, cancellationToken);
        return Ok(farms.Select(item => item.ToResponse()).ToList());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(FarmResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<FarmResponse>> Create(CreateFarmRequest request, CancellationToken cancellationToken)
    {
        var farm = new Farm
        {
            Name = request.Name,
            OriginId = request.OriginId,
            ProducerName = request.ProducerName,
            Description = request.Description
        };

        var created = await farmService.CreateAsync(farm, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.ToResponse());
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(FarmResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FarmResponse>> Update(Guid id, UpdateFarmRequest request, CancellationToken cancellationToken)
    {
        var updated = await farmService.UpdateAsync(
            new Farm
            {
                Id = id,
                Name = request.Name,
                OriginId = request.OriginId,
                ProducerName = request.ProducerName,
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
        var deleted = await farmService.SoftDeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
