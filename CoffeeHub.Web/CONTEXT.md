# CoffeeHub.Web - Technical Context

**Dependencies:** `CoffeeHub.Application`, `CoffeeHub.Infrastructure`  
**Root Reference:** [Global Context](../CONTEXT.md)

## Mission
Full-stack monolithic frontend embedded application running via **ASP.NET Core Razor Pages**. Native server-side rendering and routing acting as MVC focused tightly on the UI.

## Base Flows and Components (`/Pages`)
- Pages are embedded `.cshtml` instances with bound `PageModel` classes coupled to DI via constructor, operating on simulated REST/Local requests.
- Navigation crosses categorized views (`/Home`, `/Coffees`, `/Account`, etc).
- Interfaces populate complex dropdowns by consuming entity lists via the Application layer, with a focus on UX for crude edits (Recipe Crud, etc).
- The `/Home` feed is paginated and rich, merging data visual aggregations and Coffee references.

## Auth and Security
- Local engine focused on `Cookie Authentication` middleware in `/Security`. Authenticators are injected in the Login Pipeline with clean cookie dismantling on logout.
- Root redirection `/` routes straight to the Login Page, blocking anonymous visitors from accessing the feeds.

## Internationalization (i18n Localization)
- Translation Base: Primary implementation for English (Default) and PT-BR mapped in the `Resources/` block.
- Persistence Component: The `CultureController` listens for changes from the Navbar and sets a local priority cookie in the Agent/User's browser: `.CoffeeHub.Culture`.
- DI Inject in Views: Dynamic variables pull string translations across the global pipeline referencing `.cshtml` files via the standard `IStringLocalizer` library imported in `_ViewImports`.
- Data Annotations & Validation: FormModels and InputModels use keys linked to SharedResources (e.g., `[Required(ErrorMessage="ValidationRequired")]`) to output localized alerts. Where imperative validation is required in models, the `IStringLocalizer` is injected locally via `PageModel` and delegated cleanly to the entity's `TryBuild` method to avoid domain-scope registration forcing.

## Complexity Restrictions
- No direct access to DBContext / EF Core (a visual and structural prohibition). Validations restricted to aesthetics. Base logic delegates back to API Exceptions from *Application Services* or is blocked locally by UI rules.
