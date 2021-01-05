# ASP.NET-eShop

![.NET Core](https://github.com/gchurch/Ganges/workflows/.NET%20Core/badge.svg?branch=master)

I have created a CRUD E-commerce website using ASP.NET Core. I have created two different front-ends, one with Angular and another with Razor. The solution has a clean architecture which results in the two front-ends being easily substitutable. I have also created unit tests and functional tests for the application.

## Angular Front-end

I created an SPA front-end with Angular. The code for this is in the Web.Angular project.

The angular app uses a back-end API created with ASP.NET Core MVC.

The application has the following API:

| API                       | Description                | Request body | Response body     |
| ------------------------- | -------------------------- | ------------ | ----------------- |
| GET /api/Products         | Get all products           | None         | Array of products |
| GET /api/Products/{id}    | Get a product by ID        | None         | Product           |
| POST /api/Products        | Add a new product          | Product      | Product           |
| PUT /api/Products         | Update an existing product | Product      | Product           |
| DELETE /api/Products/{id} | Delete a product           | None         | None              |

Swagger is used to document the API: https://webangular20201103115618.azurewebsites.net/swagger/index.html

## Razor Front-end

I also created a separate front-end using ASP.NET Core MVC and Razor. The code for this is in the Web.Razor project.

## Clean Architecture

I use a Clean Architecture based on https://github.com/ardalis/CleanArchitecture to organize the code.

The Clean Architecture separates the application into three layers with each layer being a separate project. The layers are the ApplicationCore, Infrastructure and Web (User Interface). ApplicationCore contains the Entities, interfaces and services; Infrastructure contains the database code; and Web contains the controllers and front-end code. Using a clean architecture results in the Angular front-end and Razor front-end being easily substitutable. You just need to set the front-end project that you want to run as the startup project.

<img src="https://miro.medium.com/max/2750/0*lwCWXSNctrUUYeLR.png" alt="alt text" width="60%">

## Database

An SQL Server database is used to store the application data. Entity Framework Core is used for data access. LINQ is used to query data.

## Unit Tests

Unit testing is performed with MSTest using the Shouldly assertion framework and the Moq mocking framework. I have tried to follow best practice when it comes to unit testing.

## Functional Tests

The WebApplicationFactory class is used to perform functional end to end tests. A test server is created and an in-memory test database is used.
