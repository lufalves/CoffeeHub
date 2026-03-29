# Create Coffee

## Goal

Allow a coffee to be registered in the system.

## Current Flow

`Api/Web -> CoffeeService -> ICoffeeRepository -> CoffeeRepository -> DbContext -> PostgreSQL`

## Main Rules

- coffee barcode is required
- coffee barcode must be unique
- coffee name is required
- `RoasteryId` is required
- description is optional
- invalid data should be rejected before persistence

## Current Notes

The model currently supports optional references to:

- `Origin`
- `Farm`
- `BeanVariety`
- `RoastLevel`

The barcode rule is important because it acts as the first strong protection against duplicate coffee records in the catalog.
