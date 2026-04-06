using CoffeeHub.Api.Contracts.Requests;
using CoffeeHub.Api.Contracts.Responses;
using CoffeeHub.Api.Mapping;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeHub.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class ReviewsController(IReviewService reviewService, IUserService userService) : ControllerBase
{
    /// <summary>
    /// Gets all reviews.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ReviewResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ReviewResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var pagedResult = await reviewService.GetByUserIdAsync(Guid.Empty, cancellationToken);
        return Ok(await MapReviewsAsync(pagedResult, cancellationToken));
    }

    /// <summary>
    /// Gets a review by identifier.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ReviewResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReviewResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var review = await reviewService.GetByIdAsync(id, cancellationToken);

        if (review is null)
        {
            return NotFound();
        }

        var user = await userService.GetByIdAsync(review.UserId, cancellationToken);
        return Ok(review.ToResponse(user?.Name));
    }

    /// <summary>
    /// Gets all reviews for a coffee.
    /// </summary>
    [HttpGet("coffee/{coffeeId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<ReviewResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ReviewResponse>>> GetByCoffee(Guid coffeeId, CancellationToken cancellationToken)
    {
        var reviews = await reviewService.GetByCoffeeIdAsync(coffeeId, cancellationToken);
        return Ok(await MapReviewsAsync(reviews, cancellationToken));
    }

    /// <summary>
    /// Creates a review.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ReviewResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<ReviewResponse>> Create(CreateReviewRequest request, CancellationToken cancellationToken)
    {
        var review = new Review
        {
            UserId = request.UserId,
            CoffeeId = request.CoffeeId,
            Rating = request.Rating,
            Comment = request.Comment
        };

        var createdReview = await reviewService.CreateAsync(review, cancellationToken);
        var user = await userService.GetByIdAsync(createdReview.UserId, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = createdReview.Id }, createdReview.ToResponse(user?.Name));
    }

    /// <summary>
    /// Updates a review.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ReviewResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReviewResponse>> Update(Guid id, UpdateReviewRequest request, CancellationToken cancellationToken)
    {
        var updatedReview = await reviewService.UpdateAsync(
            new Review
            {
                Id = id,
                UserId = request.UserId,
                CoffeeId = request.CoffeeId,
                Rating = request.Rating,
                Comment = request.Comment
            },
            cancellationToken);

        if (updatedReview is null)
        {
            return NotFound();
        }

        var user = await userService.GetByIdAsync(updatedReview.UserId, cancellationToken);
        return Ok(updatedReview.ToResponse(user?.Name));
    }

    /// <summary>
    /// Soft-deletes a review.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await reviewService.SoftDeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    /// <summary>
    /// Gets average rating for a coffee.
    /// </summary>
    [HttpGet("coffee/{coffeeId:guid}/average")]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
    public async Task<ActionResult<decimal?>> GetAverage(Guid coffeeId, CancellationToken cancellationToken)
    {
        var average = await reviewService.GetAverageRatingAsync(coffeeId, cancellationToken);
        return Ok(average);
    }

    private async Task<IReadOnlyList<ReviewResponse>> MapReviewsAsync(IReadOnlyList<Review> reviews, CancellationToken cancellationToken)
    {
        var userIds = reviews.Select(item => item.UserId).Distinct().ToList();
        var userNames = new Dictionary<Guid, string?>(userIds.Count);

        foreach (var userId in userIds)
        {
            var user = await userService.GetByIdAsync(userId, cancellationToken);
            userNames[userId] = user?.Name;
        }

        return reviews.Select(review => review.ToResponse(userNames.GetValueOrDefault(review.UserId))).ToList();
    }
}
