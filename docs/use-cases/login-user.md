# Login User

## Goal

Allow an existing user to authenticate in the web application.

## Current Flow

`Login Razor Page -> AuthService -> IUserRepository -> UserRepository -> PostgreSQL -> Cookie SignIn`

## Main Rules

- email is required
- password is required
- the user must exist
- the provided password must match the stored hash

## Current Result

After a successful login:

- the user is authenticated with cookie authentication
- the user can access `/Home`
- the navigation changes to show authenticated actions such as logout

## Notes

This flow is already implemented in the web application.
