# Hotel search

Hotel search is a simple application that support CRUD operation on hotel entity.

Additionally, it support search of hotels by price and by location.

All units follows International System of Units (SI) if not stated otherwise.

Solution is devided in:

## Domain
All definition of domain objects including entity, validation, command, queris, etc.

## Application
Orchestraion service of application used to validate, store and get entites.

## Web api
Entry point to application. Api's are exposed on Hotels url.
For all CRUD actions user needs to be authenticated.
Search method is allowed for anonymus user.

#### Api access
Search api endpoint is available without authentication.<br /> 
For Upsert/Delete endpoint, user needs to be authenticated. <br> 
Access token can be obtained via Login method; for usernem/passord you can use any string.
There is a Swagger interface exposed on app_url/swagger/index.html.

#### Seed init hotels
For testing purpose, hotels can be seed in Program.cs file in ApplicationStarted event; just comment `return` line so seed routine is kicked.

## Test
Unit tests for Domain and Application layer.
Integration tests are in plan :)


## Tech stack

- Framework: .NET Core 9
- Language: C#
- Validations: FluentValidations 
- Testing framework: Nunit + Moq + FluentAssertions
- Data persistence storage: in-memory

