# Home Feed

## Goal

Provide an authenticated home page that behaves like a basic coffee feed.

## Current Flow

`Home Razor Page -> ICoffeeService -> ICoffeeRepository -> PostgreSQL`

## Current Behavior

- the page requires authentication
- it loads all coffees ordered by name
- it displays the current catalog count
- it renders a simple card-based feed
- if no coffees exist, it shows an empty state

## Notes

This is the first step toward a more Vivino-like experience.

Future improvements may include:

- recommended coffees
- top-rated coffees
- personalized sections
- richer metadata with roastery and origin names instead of raw IDs
