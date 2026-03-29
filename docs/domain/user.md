# User

## Meaning

Represents a person who can access the platform and interact with coffees.

## Current Responsibilities

- identify the person in the system
- store basic profile data
- support authentication-related fields

## Current Authentication Role

The current `User` model is already used for:

- registration
- password hashing
- login
- cookie-based authentication in the web application

## Expected Future Relationships

- reviews created by the user
- recipes created by the user
- favorite coffees
- followed roasteries or coffee shops

## Current Notes

The model is still intentionally small, but it is already active in the authentication flow instead of being only a placeholder.
