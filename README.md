# ASP.NET-eShop

![.NET Core](https://github.com/gchurch/Ganges/workflows/.NET%20Core/badge.svg?branch=master)

I have created a simple CRUD E-commerce website using ASP.NET Core. I have created two different front-ends, one with Angular and another with MVC and Razor. The solution has a clean architecture which results in the two front-ends being easily interchangable. I have also created unit tests and functional tests for the application.

## Angular Front-end

I created an SPA front-end with Angular. The code for this is in the Web.Angular project.

The app is deployed on Azure and can be found here: https://webangular20201103115618.azurewebsites.net/products

The angular app uses a back-end API created with ASP.NET Core.

The application has the following API:

| API                       | Description                | Request body | Response body     |
| ------------------------- | -------------------------- | ------------ | ----------------- |
| GET /api/Products         | Get all products           | None         | Array of products |
| GET /api/Products/{id}    | Get a product by ID        | None         | Product           |
| POST /api/Products        | Add a new product          | Product      | Product           |
| PUT /api/Products         | Update an existing product | Product      | Product           |
| DELETE /api/Products/{id} | Delete a product           | None         | None              |

Swagger is used to document the API: https://webangular20201103115618.azurewebsites.net/swagger/index.html

## MVC/Razor Front-end

I also created a separate front-end using ASP.NET Core MVC and Razor. The code for this is in the Web.MVC project.

This app is also deployed on Azure and can be found here: https://webmvc20201103132037.azurewebsites.net/Products

## Clean Architecture

I use a Clean Architecture based on https://github.com/ardalis/CleanArchitecture to organize the code.

The Clean Architecture separates the application into three layers with each layer being a separate project. The layers are the ApplicationCore, Infrastructure and the front-end Web. Using a clean architecture means that the Angular front-end and MVC/Razor front-end are easily interchangable. You just need to set the front-end project you want to run as the startup project.

## Database

An SQL Server database is used to store the application data. Entity Framework Core is used for data access. LINQ is used to query data.

## Unit Tests

Unit testing is performed with MSTest using the Shouldly assertion framework and the Moq mocking framework.

## Functional Tests

The WebApplicationFactory class is used to perform functional end to end tests. A test server is created and an in-memory test database is used.
