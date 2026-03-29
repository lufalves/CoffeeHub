# CoffeeHub.Web

This project contains the Razor Pages web application of CoffeeHub.

## Responsibility

`CoffeeHub.Web` is responsible for the server-rendered web interface of the platform.

It should focus on presentation and user interaction, not on infrastructure or business orchestration.

## What Belongs Here

- Razor Pages
- page models
- UI composition
- form handling
- cookie authentication
- presentation concerns

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

The web project already contains:

- login page
- registration page
- logout flow
- cookie-based authentication
- authenticated home page
- a basic coffee catalog feed on the home page

## Why Razor Pages

Razor Pages is a good fit for this project because:

- it is page-oriented
- it works well for forms and CRUD flows
- it keeps the web app simple while the product is still evolving

## Current Notes

- `/` redirects to login
- `/Home` requires authentication
- the current home page lists coffees in a general catalog-style feed
- richer feed logic such as recommendations and rankings can be added later

## Goal

Keep the web project focused on UI and user experience while relying on the application layer for rules and orchestration.
