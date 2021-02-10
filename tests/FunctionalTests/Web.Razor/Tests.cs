using ApplicationCore.Entities;
using Web.Razor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FunctionalTests.Web.Razor
{
    [TestClass]
    public class Tests : FunctionalTests<Startup>
    {

        [DataTestMethod]
        [DataRow("Products")]
        [DataRow("Products/Create")]
        [DataRow("Products/Details/1")]
        [DataRow("Products/Edit/1")]
        [DataRow("Products/Delete/1")]
        [DataRow("Products/Details/2")]
        [DataRow("Products/Edit/2")]
        [DataRow("Products/Delete/2")]
        [DataRow("Products/Details/3")]
        [DataRow("Products/Edit/3")]
        [DataRow("Products/Delete/3")]
        public async Task GetRequestOfExistingProducts_ShouldGiveOkResponse(string url)
        {
            // Arrange
            HttpClient client = _factory.CreateClient();

            // Act
            HttpResponseMessage getResponse = await client.GetAsync(url);

            // Assert
            getResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task CreatingANewProductWithAPostRequest_ShouldResultInSuccessfulGetRequest()
        {
            // Arrange
            HttpClient client = _factory.CreateClient();
            var newProduct = new Product()
            {
                Title = "TestTitle"
            };
            StringContent serializedProduct = SerializeObject(newProduct);

            // Act
            HttpResponseMessage getResponseBeforeCreation = await client.GetAsync("/Products/Details/4");
            HttpResponseMessage creationResponse = await client.PostAsync("/Products/Create", serializedProduct);
            HttpResponseMessage getResponseAfterCreation = await client.GetAsync("/Products/Details/4");

            // Assert
            getResponseBeforeCreation.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            creationResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            getResponseAfterCreation.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GettingTheDetailsOfAProductWithNonExistantID_ShouldResultInNotFoundResponse()
        {
            // Arrange
            HttpClient client = _factory.CreateClient();

            int nonExistantProductId = 4;

            // Act
            HttpResponseMessage getResponse = await client.GetAsync("/Products/Details/" + nonExistantProductId);

            // Assert
            getResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        // This test proves the the current issue that I am facing with these functional tests is that
        // we are logged in to an account. As a result, we are re-directed to the login page.
        [TestMethod]
        public async Task DeletingAProductWhenNotAuthorized_ShouldReturnForbiddenResponse()
        {
            // Arrange
            HttpClient client = _factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                }
            );

            int productId = 4;
            StringContent serializedProductId = SerializeObject(productId);

            // Act
            HttpResponseMessage deletionResponse = await client.PostAsync("/Products/Delete/" + productId, serializedProductId);

            // Assert
            deletionResponse.StatusCode.ShouldBe(HttpStatusCode.Redirect);
            deletionResponse.Headers.Location.OriginalString.ShouldStartWith("http://localhost/Identity/Account/Login");
        }

        [TestMethod]
        public async Task DeletingAProduct_AfterwardsShouldResultInANotFoundResponseWhenGettingTheProduct()
        {
            // Arrange
            HttpClient client = _factory.CreateClient();

            int productId = 3;
            StringContent serializedProductId = SerializeObject(productId);

            // Act
            HttpResponseMessage getResponseBeforeDeletion = await client.GetAsync("Products/Details/" + productId);
            HttpResponseMessage deletionResponse = await client.PostAsync("/Products/Delete/" + productId, serializedProductId);
            HttpResponseMessage getResponseAfterDeletion = await client.GetAsync("/Products/Details/" + productId);

            // Assert
            getResponseBeforeDeletion.StatusCode.ShouldBe(HttpStatusCode.OK);
            deletionResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            getResponseAfterDeletion.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task EditingAnExistingProductWithAPostRequest_ShouldRespondWithAnOkStatusCode()
        {
            // Arrange
            HttpClient client = _factory.CreateClient();

            var existingProduct = new Product()
            {
                ProductId = 3,
            };
            StringContent serializedProduct = SerializeObject(existingProduct);

            // Act
            HttpResponseMessage getResponse = await client.PostAsync("Products/Edit/" + existingProduct.ProductId, serializedProduct);

            // Assert
            getResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task EditingANonExistentProductWithAPostRequest_ShouldRespondWithANotFoundStatusCode()
        {
            // Arrange
            HttpClient client = _factory.CreateClient();

            var nonExistentProduct = new Product()
            {
                ProductId = 0,
            };
            StringContent serializedProduct = SerializeObject(nonExistentProduct);

            // Act
            HttpResponseMessage getResponse = await client.PostAsync("Products/Edit", serializedProduct);

            // Assert
            getResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

    }
}
