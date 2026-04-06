using CoffeeHub.Api.Contracts.Responses;
using CoffeeHub.Application.Common;
using CoffeeHub.Domain.BeanVariety;
using CoffeeHub.Domain.BrewingMethod;
using CoffeeHub.Domain.Coffee;
using CoffeeHub.Domain.Farm;
using CoffeeHub.Domain.Origin;
using CoffeeHub.Domain.Recipe;
using CoffeeHub.Domain.Review;
using CoffeeHub.Domain.RoastLevel;
using CoffeeHub.Domain.Roastery;
using CoffeeHub.Domain.User;

namespace CoffeeHub.Api.Mapping;

public static class ApiResponseMapper
{
    public static CoffeeResponse ToResponse(this Coffee coffee)
    {
        return new CoffeeResponse(
            coffee.Id,
            coffee.Barcode,
            coffee.Name,
            coffee.Description,
            coffee.RoasteryId,
            coffee.Roastery?.Name,
            coffee.OriginId,
            coffee.Origin?.Country,
            coffee.FarmId,
            coffee.Farm?.Name,
            coffee.BeanVarietyId,
            coffee.BeanVariety?.Name,
            coffee.RoastLevelId,
            coffee.RoastLevel?.Name,
            coffee.CreatedAt,
            coffee.UpdatedAt);
    }

    public static UserResponse ToResponse(this User user)
    {
        return new UserResponse(
            user.Id,
            user.Name,
            user.Email,
            user.AvatarUrl,
            user.Role,
            user.CreatedAt,
            user.UpdatedAt);
    }

    public static RecipeResponse ToResponse(this Recipe recipe)
    {
        return new RecipeResponse(
            recipe.Id,
            recipe.UserId,
            recipe.CoffeeId,
            recipe.Coffee?.Name,
            recipe.BrewingMethodId,
            recipe.BrewingMethod?.Name,
            recipe.Title,
            recipe.Description,
            recipe.CoffeeAmountInGrams,
            recipe.WaterAmountInMilliliters,
            recipe.BrewTimeInSeconds,
            recipe.Instructions,
            recipe.CreatedAt,
            recipe.UpdatedAt);
    }

    public static ReviewResponse ToResponse(this Review review, string? userName)
    {
        return new ReviewResponse(
            review.Id,
            review.UserId,
            userName,
            review.CoffeeId,
            review.Coffee?.Name,
            review.Rating,
            review.Comment,
            review.CreatedAt,
            review.UpdatedAt);
    }

    public static RoasteryResponse ToResponse(this Roastery roastery)
    {
        return new RoasteryResponse(
            roastery.Id,
            roastery.Name,
            roastery.Description,
            roastery.WebsiteUrl,
            roastery.InstagramUrl,
            roastery.LogoUrl,
            roastery.CreatedAt,
            roastery.UpdatedAt);
    }

    public static OriginResponse ToResponse(this Origin origin)
    {
        return new OriginResponse(
            origin.Id,
            origin.Country,
            origin.Region,
            origin.Locality,
            origin.Description,
            origin.CreatedAt,
            origin.UpdatedAt);
    }

    public static FarmResponse ToResponse(this Farm farm)
    {
        return new FarmResponse(
            farm.Id,
            farm.Name,
            farm.OriginId,
            farm.Origin?.Country,
            farm.ProducerName,
            farm.Description,
            farm.CreatedAt,
            farm.UpdatedAt);
    }

    public static BeanVarietyResponse ToResponse(this BeanVariety beanVariety)
    {
        return new BeanVarietyResponse(
            beanVariety.Id,
            beanVariety.Name,
            beanVariety.Description,
            beanVariety.CreatedAt,
            beanVariety.UpdatedAt);
    }

    public static RoastLevelResponse ToResponse(this RoastLevel roastLevel)
    {
        return new RoastLevelResponse(
            roastLevel.Id,
            roastLevel.Name,
            roastLevel.Description,
            roastLevel.DisplayOrder,
            roastLevel.CreatedAt,
            roastLevel.UpdatedAt);
    }

    public static BrewingMethodResponse ToResponse(this BrewingMethod brewingMethod)
    {
        return new BrewingMethodResponse(
            brewingMethod.Id,
            brewingMethod.Name,
            brewingMethod.Description,
            brewingMethod.CreatedAt,
            brewingMethod.UpdatedAt);
    }

    public static PagedResponse<TResponse> ToPagedResponse<T, TResponse>(this PagedResult<T> pagedResult, Func<T, TResponse> mapper)
    {
        var mappedItems = pagedResult.Items.Select(mapper).ToList();
        var totalPages = pagedResult.TotalCount == 0
            ? 0
            : (int)Math.Ceiling((double)pagedResult.TotalCount / pagedResult.PageSize);

        return new PagedResponse<TResponse>(
            mappedItems,
            pagedResult.Page,
            pagedResult.PageSize,
            pagedResult.TotalCount,
            totalPages);
    }
}
