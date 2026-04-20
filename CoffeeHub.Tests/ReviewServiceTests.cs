using CoffeeHub.Application.Services;
using CoffeeHub.Tests.Builders;
using CoffeeHub.Tests.Common;
using CoffeeHub.Tests.Fakes;

namespace CoffeeHub.Tests;

public class ReviewServiceTests : ServiceTestBase
{
    [Fact]
    public async Task CreateAsync_ShouldCreateReview_WhenDataIsValid()
    {
        var repository = new FakeReviewRepository();
        var service = new ReviewService(repository, Logger<ReviewService>());

        var review = new TestReviewBuilder().Build();

        var created = await service.CreateAsync(review, TestContext.Current.CancellationToken);

        Assert.Same(review, created);
        Assert.Single(repository.Reviews);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenUserIdIsEmpty()
    {
        var repository = new FakeReviewRepository();
        var service = new ReviewService(repository, Logger<ReviewService>());

        var review = new TestReviewBuilder().WithUserId(Guid.Empty).Build();

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateAsync(review, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenCoffeeIdIsEmpty()
    {
        var repository = new FakeReviewRepository();
        var service = new ReviewService(repository, Logger<ReviewService>());

        var review = new TestReviewBuilder().WithCoffeeId(Guid.Empty).Build();

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateAsync(review, TestContext.Current.CancellationToken));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(0.5)]
    public async Task CreateAsync_ShouldThrow_WhenRatingIsBelow1(decimal rating)
    {
        var repository = new FakeReviewRepository();
        var service = new ReviewService(repository, Logger<ReviewService>());

        var review = new TestReviewBuilder().WithRating(rating).Build();

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateAsync(review, TestContext.Current.CancellationToken));
    }

    [Theory]
    [InlineData(6)]
    [InlineData(5.1)]
    [InlineData(10)]
    public async Task CreateAsync_ShouldThrow_WhenRatingIsAbove5(decimal rating)
    {
        var repository = new FakeReviewRepository();
        var service = new ReviewService(repository, Logger<ReviewService>());

        var review = new TestReviewBuilder().WithRating(rating).Build();

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateAsync(review, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenCommentExceeds2000Chars()
    {
        var repository = new FakeReviewRepository();
        var service = new ReviewService(repository, Logger<ReviewService>());

        var review = new TestReviewBuilder().WithComment(new string('C', 2001)).Build();

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateAsync(review, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenReviewDoesNotExist()
    {
        var repository = new FakeReviewRepository();
        var service = new ReviewService(repository, Logger<ReviewService>());

        var review = new TestReviewBuilder().WithId(Guid.NewGuid()).Build();

        var result = await service.UpdateAsync(review, TestContext.Current.CancellationToken);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateReview_WhenReviewExists()
    {
        var existing = new TestReviewBuilder().WithId(Guid.NewGuid()).WithRating(3).Build();

        var repository = new FakeReviewRepository();
        repository.Reviews.Add(existing);

        var service = new ReviewService(repository, Logger<ReviewService>());

        var update = new TestReviewBuilder()
            .WithId(existing.Id)
            .WithRating(5)
            .WithComment("Updated comment")
            .Build();

        var result = await service.UpdateAsync(update, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.Equal(5, result.Rating);
        Assert.Equal("Updated comment", result.Comment);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public async Task UpdateAsync_ShouldThrow_WhenRatingIsInvalid(decimal rating)
    {
        var existing = new TestReviewBuilder().WithId(Guid.NewGuid()).Build();

        var repository = new FakeReviewRepository();
        repository.Reviews.Add(existing);

        var service = new ReviewService(repository, Logger<ReviewService>());

        var update = new TestReviewBuilder()
            .WithId(existing.Id)
            .WithRating(rating)
            .Build();

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.UpdateAsync(update, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task SoftDeleteAsync_ShouldReturnFalse_WhenReviewDoesNotExist()
    {
        var repository = new FakeReviewRepository();
        var service = new ReviewService(repository, Logger<ReviewService>());

        var result = await service.SoftDeleteAsync(Guid.NewGuid(), TestContext.Current.CancellationToken);

        Assert.False(result);
        Assert.Null(repository.SoftDeletedReviewId);
    }

    [Fact]
    public async Task SoftDeleteAsync_ShouldDelete_WhenReviewExists()
    {
        var existing = new TestReviewBuilder().WithId(Guid.NewGuid()).Build();

        var repository = new FakeReviewRepository();
        repository.Reviews.Add(existing);

        var service = new ReviewService(repository, Logger<ReviewService>());

        var result = await service.SoftDeleteAsync(existing.Id, TestContext.Current.CancellationToken);

        Assert.True(result);
        Assert.Equal(existing.Id, repository.SoftDeletedReviewId);
    }
}
