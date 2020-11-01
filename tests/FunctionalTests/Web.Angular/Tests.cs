using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Threading.Tasks;
using System.Net;
using ApplicationCore.Entities;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Web.Angular;
using System;

namespace FunctionalTests.Web.Angular
{

    // Useful information: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.1

    // These tests could also be described as end-to-end functional tests.

    [TestClass]
    public class Tests : FunctionalTests<Startup>
    {

        [DataTestMethod]
        [DataRow("/api/products")]
        [DataRow("/api/products/1")]
        [DataRow("/api/products/2")]
        [DataRow("/api/products/3")]
        public async Task GetRequestsOfExistingProducts_ShouldGiveOkResponse(string url)
        {
            // Arrange
            HttpClient client = CreateTestHttpClient();

            // Act
            HttpResponseMessage response = await client.GetAsync(url);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GettingAllProductsWithAGetRequest_ShouldReturnOkStatusCodeAndAllTheProducts()
        {
            // Arrange
            HttpClient client = CreateTestHttpClient();

            // Act
            HttpResponseMessage response = await client.GetAsync("/api/products");
            Product[] responseProducts = await DeserializeResponse<Product[]>(response);

            // Assert
            responseProducts.Length.ShouldBe(3);
            responseProducts[0].Id.ShouldBe(1);
            responseProducts[0].Title.ShouldBe("Toy");
            responseProducts[1].Id.ShouldBe(2);
            responseProducts[1].Title.ShouldBe("Book");
            responseProducts[2].Id.ShouldBe(3);
            responseProducts[2].Title.ShouldBe("Lamp");
        }

        [TestMethod]
        public async Task GettingAProductWithAGetRequest_GivenAProductIdThatExists_ShouldReturnOkStatusCodeAndTheRequestedProduct()
        {
            // Arrange
            HttpClient client = CreateTestHttpClient();
            int productIdThatExists = 2;

            // Act
            HttpResponseMessage response = await client.GetAsync("/api/products/" + productIdThatExists);
            Product responseProduct = await DeserializeResponse<Product>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            responseProduct.Id.ShouldBe(2);
            responseProduct.Title.ShouldBe("Book");
            responseProduct.Description.ShouldBe("Hard back");
            responseProduct.Seller.ShouldBe("Peter");
            responseProduct.Price.ShouldBe(25);
            responseProduct.Quantity.ShouldBe(4);
            responseProduct.ImageUrl.ShouldBe("book.png");
        }

        [TestMethod]
        public async Task AddingAProductWithAPostRequest_GivenAValidProduct_ShouldReturnCreatedStatusCodeAndTheCreatedProduct()
        {
            // Arrange
            HttpClient client = CreateTestHttpClient();

            var product = new Product()
            {
                Id = 0,
                Title = "Sock",
                Description = "Red",
                Seller = "Anthony",
                Price = 2,
                Quantity = 3,
            };
            StringContent serializedProduct = SerializeObject(product);

            // Act
            HttpResponseMessage response = await client.PostAsync("/api/products/", serializedProduct);
            Product responseProduct = await DeserializeResponse<Product>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            responseProduct.Id.ShouldNotBe(0);
            responseProduct.Title.ShouldBe(product.Title);
            responseProduct.Description.ShouldBe(product.Description);
            responseProduct.Seller.ShouldBe(product.Seller);
            responseProduct.Price.ShouldBe(product.Price);
            responseProduct.Quantity.ShouldBe(product.Quantity);
        }

        [TestMethod]
        public async Task AddingAProductWithAPostRequest_GivenAValidProduct_ShouldEnableSuccessfulGetRequest()
        {
            // Arrange
            HttpClient client = CreateTestHttpClient();
            var product = new Product()
            {
                Id = 0,
                Title = "Sock",
                Description = "Red",
                Seller = "Anthony",
                Price = 2,
                Quantity = 3,
            };
            StringContent serializedProduct = SerializeObject(product);

            // Act
            HttpResponseMessage postResponse = await client.PostAsync("/api/products/", serializedProduct);
            Product postResponseProduct = await DeserializeResponse<Product>(postResponse);

            HttpResponseMessage getResponse = await client.GetAsync("/api/products/" + postResponseProduct.Id);
            Product getResponseProduct = await DeserializeResponse<Product>(getResponse);

            // Assert
            getResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            getResponseProduct.Id.ShouldBe(postResponseProduct.Id);
            getResponseProduct.Title.ShouldBe(product.Title);
            getResponseProduct.Description.ShouldBe(product.Description);
            getResponseProduct.Seller.ShouldBe(product.Seller);
            getResponseProduct.Price.ShouldBe(product.Price);
            getResponseProduct.Quantity.ShouldBe(product.Quantity);
        }

        [TestMethod]
        public async Task UpdatingAProductWithAPutRequest_GivenAValidProduct_ShouldReturnOkStatusCodeAndTheUpdatedProduct()
        {
            // Arrange
            HttpClient client = CreateTestHttpClient();
            var product = new Product()
            {
                Id = 1,
                Title = "New Title",
                Description = "New Description",
                Seller = "New Seller",
                Price = 100,
                Quantity = 20,
            };
            StringContent serializedProduct = SerializeObject(product);

            // Act
            HttpResponseMessage response = await client.PutAsync("/api/products/", serializedProduct);
            Product responseProduct = await DeserializeResponse<Product>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            responseProduct.Id.ShouldBe(product.Id);
            responseProduct.Title.ShouldBe(product.Title);
            responseProduct.Description.ShouldBe(product.Description);
            responseProduct.Seller.ShouldBe(product.Seller);
            responseProduct.Price.ShouldBe(product.Price);
            responseProduct.Quantity.ShouldBe(product.Quantity);
        }

        [TestMethod]
        public async Task DeletingAProductWithADeleteRequest_GivenAProductIdThatExists_ShouldStopSuccessfulGetRequestOfThatProduct()
        {
            // Arrange
            HttpClient client = CreateTestHttpClient();
            int id = 1;
            string url = "/api/products/" + id;

            // Act
            HttpResponseMessage getResponseBeforeDeletion = await client.GetAsync(url);
            HttpResponseMessage deletionResponse = await client.DeleteAsync(url);
            HttpResponseMessage getResponseAfterDeletion = await client.GetAsync(url);

            // Assert
            getResponseBeforeDeletion.StatusCode.ShouldBe(HttpStatusCode.OK);
            deletionResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            getResponseAfterDeletion.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}
