# CoffeeHub Docs

This folder contains documentation that is more detailed than the project `README.md` files.

## Purpose

The `README.md` files describe architecture, responsibilities, and conventions.

The `docs` folder exists for:

- functional documentation
- use cases
- domain notes
- technical decisions
- business rules that do not belong in project overviews

## Current Focus

The documentation currently reflects:

- registration and login flow in the web app
- the general coffee feed on the authenticated home page
- the rule that each coffee must have a unique barcode
- the architectural separation between services and repositories

## Suggested Structure

- `use-cases`: application flows and behavior
- `domain`: entity-oriented notes and business meaning
- `decisions`: architectural or technical decisions

## Rule Of Thumb

- If the content explains a project or a layer, it belongs in a `README.md`.
- If the content explains how the system behaves, it belongs in `docs/`.
