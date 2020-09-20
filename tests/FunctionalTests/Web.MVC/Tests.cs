using Ganges.ApplicationCore.Entities;
using Ganges.Web.MVC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ganges.FunctionalTests.Web.MVC
{
    [TestClass]
    public class Tests
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
            CustomWebApplicationFactory<Startup> factory
                = new CustomWebApplicationFactory<Startup>();
            var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);


            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task CreatingANewProductWithAPostRequest_ShouldResultInSuccessfulGetRequest()
        {
            // Arrange
            CustomWebApplicationFactory<Startup> factory
                = new CustomWebApplicationFactory<Startup>();
            var client = factory.CreateClient();
            var product = new Product()
            {
                Title = "TestTitle"
            };
            var productString = JsonConvert.SerializeObject(product);
            var stringContent = new StringContent(productString, Encoding.UTF8, "application/json");

            // Act
            var getResponseBeforeCreation = await client.GetAsync("/Products/Details/4");
            var postResponse = await client.PostAsync("/Products/Create", stringContent);
            var getResponseAfterCreation = await client.GetAsync("/Products/Details/4");

            // Assert
            getResponseBeforeCreation.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            postResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            getResponseAfterCreation.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GettingTheDetailsOfAProductWithNonExistantID_ShouldResultInNotFoundResponse()
        {
            // Arrange
            CustomWebApplicationFactory<Startup> factory
                = new CustomWebApplicationFactory<Startup>();
            var client = factory.CreateClient();

            var productId = 4;

            // Act
            var getResponse = await client.GetAsync("/Products/Details/" + productId);

            // Assert
            getResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task DeletingAProduct_ShouldResultInANotFoundResponseWhenGettingTheProduct()
        {
            // Arrange
            CustomWebApplicationFactory<Startup> factory
                = new CustomWebApplicationFactory<Startup>();
            var client = factory.CreateClient();

            var productId = 3;
            var productString = JsonConvert.SerializeObject(productId);
            var stringContent = new StringContent(productString, Encoding.UTF8, "application/json");

            // Act
            var getResponseBeforeDeletion = await client.GetAsync("Products/Details/" + productId);
            var deletionResponse = await client.PostAsync("/Products/Delete/" + productId, stringContent);
            var getResponseAfterDeletion = await client.GetAsync("/Products/Details/" + productId);

            // Assert
            getResponseBeforeDeletion.StatusCode.ShouldBe(HttpStatusCode.OK);
            deletionResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            getResponseAfterDeletion.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task EditingAnExistingProductWithAPostRequest_ShouldRespondWithAnOkStatusCode()
        {
            // Arrange
            CustomWebApplicationFactory<Startup> factory
                = new CustomWebApplicationFactory<Startup>();
            var client = factory.CreateClient();

            var newProduct = new Product()
            {
                Id = 3,
            };
            var productString = JsonConvert.SerializeObject(newProduct);
            var stringContent = new StringContent(productString, Encoding.UTF8, "application/json");

            // Act
            var getResponse = await client.PostAsync("Products/Edit/" + newProduct.Id, stringContent);

            // Assert
            getResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task EditingANonExistentProductWithAPostRequest_ShouldRespondWithANotFoundStatusCode()
        {
            // Arrange
            CustomWebApplicationFactory<Startup> factory
                = new CustomWebApplicationFactory<Startup>();
            var client = factory.CreateClient();

            var nonExistentProduct = new Product()
            {
                Id = 4,
            };
            var productString = JsonConvert.SerializeObject(nonExistentProduct);
            var stringContent = new StringContent(productString, Encoding.UTF8, "application/json");

            // Act
            var getResponse = await client.PostAsync("Products/Edit", stringContent);

            // Assert
            getResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

    }
}
