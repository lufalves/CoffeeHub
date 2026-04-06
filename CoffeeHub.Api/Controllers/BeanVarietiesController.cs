using CoffeeHub.Api.Contracts.Responses;
using CoffeeHub.Api.Mapping;
using CoffeeHub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeHub.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class BeanVarietiesController(IBeanVarietyService beanVarietyService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<BeanVarietyResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<BeanVarietyResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var beanVarieties = await beanVarietyService.GetAllAsync(cancellationToken);
        return Ok(beanVarieties.Select(item => item.ToResponse()).ToList());
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BeanVarietyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BeanVarietyResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var beanVariety = await beanVarietyService.GetByIdAsync(id, cancellationToken);
        return beanVariety is null ? NotFound() : Ok(beanVariety.ToResponse());
    }
}
