# Coffee Barcode

## Decision

Every coffee must have a barcode, and the barcode must be unique.

## Why

The barcode is the first strong practical safeguard against duplicated coffee records.

Without it, the catalog can quickly accumulate repeated entries for the same product with slightly different names or descriptions.

## Current Enforcement

The rule is enforced in multiple layers:

- domain model includes the `Barcode` field
- application service validates presence and duplicate usage
- EF Core mapping defines the field as required
- PostgreSQL has a unique index for the barcode

## Notes

This does not prevent future improvements such as more advanced duplicate detection, but it establishes a reliable base rule for catalog integrity.
