using CoffeeHub.Api.Contracts.Responses;
using CoffeeHub.Api.Mapping;
using CoffeeHub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeHub.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class BrewingMethodsController(IBrewingMethodService brewingMethodService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<BrewingMethodResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<BrewingMethodResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var methods = await brewingMethodService.GetAllOrderedAsync(cancellationToken);
        return Ok(methods.Select(item => item.ToResponse()).ToList());
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BrewingMethodResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BrewingMethodResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var method = await brewingMethodService.GetByIdAsync(id, cancellationToken);
        return method is null ? NotFound() : Ok(method.ToResponse());
    }
}
