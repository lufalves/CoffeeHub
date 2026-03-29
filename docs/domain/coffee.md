# Coffee

## Meaning

Represents a coffee item that users can explore, review, and use in recipes.

## Current Responsibilities

- identify the coffee
- store barcode and descriptive information
- relate the coffee to origin and production data

## Current Relationships

- `Roastery`
- `Origin`
- `Farm`
- `BeanVariety`
- `RoastLevel`

## Important Current Rule

Each coffee must have a `Barcode`.

This barcode:

- is required
- must be unique
- helps avoid duplicated coffee records in the catalog

## Expected Future Relationships

- reviews
- recipes
- availability in coffee shops
- tasting notes
- processing method

## Current Notes

The model is still intentionally small, but the barcode rule already gives it a stronger identity in the product.
