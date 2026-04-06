using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Coffee;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoffeesController(ICoffeeService coffeeService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Coffee>>> GetAll(CancellationToken cancellationToken)
    {
        var coffees = await coffeeService.GetAllAsync(cancellationToken);
        return Ok(coffees);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Coffee>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var coffee = await coffeeService.GetByIdAsync(id, cancellationToken);

        if (coffee is null)
        {
            return NotFound();
        }

        return Ok(coffee);
    }

    [HttpPost]
    public async Task<ActionResult<Coffee>> Create(CreateCoffeeRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var coffee = new Coffee
            {
                Barcode = request.Barcode,
                Name = request.Name,
                Description = request.Description,
                RoasteryId = request.RoasteryId,
                OriginId = request.OriginId,
                FarmId = request.FarmId,
                BeanVarietyId = request.BeanVarietyId,
                RoastLevelId = request.RoastLevelId
            };

            var createdCoffee = await coffeeService.CreateAsync(coffee, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = createdCoffee.Id }, createdCoffee);
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
    public async Task<ActionResult<Coffee>> Update(Guid id, UpdateCoffeeRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var updatedCoffee = await coffeeService.UpdateAsync(
                new Coffee
                {
                    Id = id,
                    Barcode = request.Barcode,
                    Name = request.Name,
                    Description = request.Description,
                    RoasteryId = request.RoasteryId,
                    OriginId = request.OriginId,
                    FarmId = request.FarmId,
                    BeanVarietyId = request.BeanVarietyId,
                    RoastLevelId = request.RoastLevelId
                },
                cancellationToken);

            return updatedCoffee is null ? NotFound() : Ok(updatedCoffee);
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
            var deleted = await coffeeService.SoftDeleteAsync(id, cancellationToken);
            return deleted ? NoContent() : NotFound();
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }
}

public record CreateCoffeeRequest(
    string Barcode,
    string Name,
    Guid RoasteryId,
    string? Description,
    Guid? OriginId,
    Guid? FarmId,
    Guid? BeanVarietyId,
    Guid? RoastLevelId);

public record UpdateCoffeeRequest(
    string Barcode,
    string Name,
    Guid RoasteryId,
    string? Description,
    Guid? OriginId,
    Guid? FarmId,
    Guid? BeanVarietyId,
    Guid? RoastLevelId);
