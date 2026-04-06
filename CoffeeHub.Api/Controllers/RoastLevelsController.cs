using CoffeeHub.Api.Contracts.Responses;
using CoffeeHub.Api.Mapping;
using CoffeeHub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeHub.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class RoastLevelsController(IRoastLevelService roastLevelService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<RoastLevelResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<RoastLevelResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var roastLevels = await roastLevelService.GetAllOrderedAsync(cancellationToken);
        return Ok(roastLevels.Select(item => item.ToResponse()).ToList());
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RoastLevelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoastLevelResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var roastLevel = await roastLevelService.GetByIdAsync(id, cancellationToken);
        return roastLevel is null ? NotFound() : Ok(roastLevel.ToResponse());
    }
}
