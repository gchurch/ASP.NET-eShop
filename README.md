# Ganges

![.NET Core](https://github.com/gchurch/Ganges/workflows/.NET%20Core/badge.svg?branch=master)

## Front-End

An SPA E-Commerce website front-end created with Angular 9.


## Back-End

### API

I created a back-end API with ASP.NET Core 3.1.

The application has the following API:

| API                       | Description                | Request body | Response body     |
| ------------------------- | -------------------------- | ------------ | ----------------- |
| GET /api/Products         | Get all products           | None         | Array of products |
| GET /api/Products/{id}    | Get a product by ID        | None         | Product           |
| POST /api/Products        | Add a new product          | Product      | Product           |
| PUT /api/Products         | Update an existing product | Product      | Product           |
| DELETE /api/Products/{id} | Delete a product           | None         | None              |

Swagger is used to document the API.

### Database

- An SQL Server used to store application data.
- Entity Framework Core is used for data access.
- LINQ is used to query data.

### Clean Architecture

A Clean Architecure based on https://github.com/ardalis/CleanArchitecture is used to organize the code.

The application is separated into three layers where each layer is a project. The layers are called ApplicationCore, Web and Infrastructure.

<img src="https://miro.medium.com/max/2750/0*lwCWXSNctrUUYeLR.png" alt="alt text" width="60%">

### Unit Tests

Unit testing is performed with MSTest using the Shouldly assertion framework and the Moq mocking framework.

### Functional Tests

The [WebApplicationFactory](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.testing.webapplicationfactory-1?view=aspnetcore-3.0) class is used to perform functional end to end tests. A test server is created and an in-memory test database is used. Requests are made to the test server and the responses are tested.
