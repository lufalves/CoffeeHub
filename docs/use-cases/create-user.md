# Create User

## Goal

Allow a new user to be registered in the system.

## Current Flow

`Register Razor Page -> AuthService -> IUserRepository -> UserRepository -> DbContext -> PostgreSQL`

## Main Rules

- user name is required
- email is required
- email must be unique
- password is required
- password must have at least 6 characters
- password is hashed before persistence
- invalid data should be rejected before persistence

## Current Result

After a successful registration:

- the user is stored in PostgreSQL
- the password is persisted as a hash
- the web application signs the user in with cookie authentication
- the user is redirected to the authenticated home page

## Notes

This is already implemented in the web application and is no longer just a planned flow.
