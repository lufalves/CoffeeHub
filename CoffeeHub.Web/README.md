# CoffeeHub.Web

This project contains the Razor Pages web application of CoffeeHub.

## Responsibility

`CoffeeHub.Web` is responsible for the server-rendered web interface of the platform.

It should focus on presentation and user interaction, not on infrastructure or business orchestration.

## Current Structure

| Folder | Contents |
|---|---|
| `Pages/` | Razor Pages organized by feature |
| `Pages/Account/` | Login, Register, Logout, Profile management |
| `Pages/Home/` | Authenticated home page with paginated coffee feed |
| `Pages/Coffees/` | Coffee CRUD pages (create, edit, details, delete) |
| `Pages/Recipes/` | Recipe CRUD pages |
| `Pages/Reviews/` | Review CRUD pages |
| `Pages/Roasteries/` | Roastery listing and detail pages |
| `Pages/Origins/` | Origin listing and detail pages |
| `Pages/Shared/` | Layout, partials, language switcher |
| `Controllers/` | MVC controllers (CultureController for i18n) |
| `Resources/` | Localization resources (.resx files for EN and PT-BR) |
| `Security/` | Cookie authentication configuration |
| `wwwroot/` | Static assets (CSS, JS, images) |

## What Belongs Here

- Razor Pages
- page models
- UI composition
- form handling
- cookie authentication
- presentation concerns
- localization resources
- static assets

## What Should Not Be Here

- direct EF Core access
- persistence logic
- domain orchestration logic
- duplicated validation that should live in the application layer

## Expected Flow

Preferred flow:

`PageModel -> Application Service -> Repository -> Database`

For authentication:

`PageModel -> AuthService -> Repository -> PostgreSQL -> Cookie SignIn`

## Current Features

### Authentication & Authorization
- Login page with brute-force protection display
- Registration page with automatic Admin role for first user
- Logout flow
- Cookie-based authentication
- Profile page with user role badge, avatar, name, email, and password change

### Coffee Management
- Paginated home page with rich coffee information (roastery name, origin country)
- Coffee create page with dropdowns for reference data
- Coffee edit page
- Coffee details page
- Coffee delete with confirmation

### Reference Data
- Roastery listing and detail pages
- Origin listing and detail pages

### Recipes & Reviews
- Recipe listing and detail pages
- Review listing and detail pages

### Internationalization (i18n)
- English and Portuguese (PT-BR) support
- Language switcher in navbar
- Cookie-based culture persistence
- `CultureController` for switching languages

### Error Handling
- Global error page (`/Error`)
- 404 Not Found page (`/NotFound`)

## Why Razor Pages

Razor Pages is a good fit for this project because:

- it is page-oriented
- it works well for forms and CRUD flows
- it keeps the web app simple while the product is still evolving

## Current Notes

- `/` redirects to login
- `/Home` requires authentication and shows paginated coffee feed
- Reference data dropdowns are populated on create/edit forms
- Richer feed logic such as recommendations and ranking can be added later

## Goal

Keep the web project focused on UI and user experience while relying on the application layer for rules and orchestration.
