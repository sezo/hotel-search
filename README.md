# Hotel search

Hotel search is a simple application that support CRUD operation on hotel entity.

Additionally, it support search of hotels by price and by location.

Solution is devided in:

## Domain
All definition of domain objects including entity, validation, command, queris, etc.

## Application
Orchestraion service of application used to validate, store and get entites.

## Web api
Entry point to application. Api's are exposed on Hotels url.
For all CRUD actions user needs to be authenticated.
Search method is allowed for anonymus user.

For testing purpose, hotels can be seed in Program.cs file in ApplicationStarted event; just commnet return line.

## Test
Uniti test for Domain and Application layer.
Integration test are planned :)