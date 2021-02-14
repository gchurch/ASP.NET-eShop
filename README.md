# ASP.NET-eShop

![.NET Core](https://github.com/gchurch/Ganges/workflows/.NET%20Core/badge.svg?branch=master)

I have created a CRUD application in the style of an E-commerce website using ASP.NET Core 5.0. I have created a few different front-ends using different technologies. The front-end technologies I have used are Angular, React, and Razor. The solution uses a clean architecture which results in the front-ends being easily substitutable for one another. I have also created unit tests and functional tests for the application.

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

I am currently working on implementing authentication and authorization for this version of the front-end.

## React Front-end

I created a similar SPA front-end using React. The code for this is in the Web.React project.

## Razor Front-end

I also created a separate front-end using ASP.NET Core MVC and Razor. The code for this is in the Web.Razor project. For this version of the application I have implemented authentication and authorization using Identity. Users need to registered and be logged in to an account in order to create a product. A user can only edit and delete products that they created themself. The admin account can edit and delete all products.

## Clean Architecture

A Clean Architecture based on https://github.com/ardalis/CleanArchitecture is used to organize the code.

The Clean Architecture separates the application into three layers with each layer being a separate project. The layers are the ApplicationCore, Infrastructure and Web (User Interface). ApplicationCore contains the Entities, interfaces and services; Infrastructure contains the database code; and Web contains the controllers and front-end code. Using a clean architecture results in the different front-ends I created being easily substitutable for one another. You just need to set the front-end project that you want to run as the startup project.

<img src="https://miro.medium.com/max/2750/0*lwCWXSNctrUUYeLR.png" alt="alt text" width="60%">

## Database

An SQL Server database is used to store the application data. Entity Framework Core is used for data access. LINQ is used to query data.

## Unit Tests

Unit testing is performed with MSTest using the Shouldly assertion framework and the Moq mocking framework. I have tried to follow best practice when it comes to unit testing.

## Functional Tests

The WebApplicationFactory class is used to perform functional end to end tests. A test server is created and an in-memory test database is used.
