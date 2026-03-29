# Create Review

## Goal

Allow a user to review a coffee.

## Intended Flow

`Api/Web -> ReviewService -> IReviewRepository -> ReviewRepository -> DbContext -> PostgreSQL`

## Expected Rules

- a review must belong to a user
- a review must belong to a coffee
- rating is required
- comment is optional

## Notes

This use case is not implemented yet, but it is important enough to document early because it is one of the core behaviors of CoffeeHub.
