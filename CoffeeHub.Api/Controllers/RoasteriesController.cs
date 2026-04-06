using CoffeeHub.Api.Contracts.Requests;
using CoffeeHub.Api.Contracts.Responses;
using CoffeeHub.Api.Mapping;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Roastery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeHub.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class RoasteriesController(IRoasteryService roasteryService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<RoasteryResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<RoasteryResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var roasteries = await roasteryService.GetAllAsync(cancellationToken);
        return Ok(roasteries.Select(item => item.ToResponse()).ToList());
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RoasteryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoasteryResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var roastery = await roasteryService.GetByIdAsync(id, cancellationToken);
        return roastery is null ? NotFound() : Ok(roastery.ToResponse());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(RoasteryResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<RoasteryResponse>> Create(CreateRoasteryRequest request, CancellationToken cancellationToken)
    {
        var roastery = new Roastery
        {
            Name = request.Name,
            Description = request.Description,
            WebsiteUrl = request.WebsiteUrl,
            InstagramUrl = request.InstagramUrl,
            LogoUrl = request.LogoUrl
        };

        var created = await roasteryService.CreateAsync(roastery, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.ToResponse());
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(RoasteryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoasteryResponse>> Update(Guid id, UpdateRoasteryRequest request, CancellationToken cancellationToken)
    {
        var updated = await roasteryService.UpdateAsync(
            new Roastery
            {
                Id = id,
                Name = request.Name,
                Description = request.Description,
                WebsiteUrl = request.WebsiteUrl,
                InstagramUrl = request.InstagramUrl,
                LogoUrl = request.LogoUrl
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
        var deleted = await roasteryService.SoftDeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
