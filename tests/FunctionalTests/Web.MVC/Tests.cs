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
        public async Task Get_EndpointsReturnSuccess(string url)
        {
            // Arrange
            CustomWebApplicationFactory<Startup> factory
                = new CustomWebApplicationFactory<Startup>();
            var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);


            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestMethod]
        public async Task PostingNewProductShouldResultInSuccessfulGetRequest()
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
            var postResponse = await client.PostAsync("/Products/Create", stringContent);

            var getResponse = await client.GetAsync("/Products/Details/4");

            // Assert
            getResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task NotFoundTest()
        {
            // Arrange
            CustomWebApplicationFactory<Startup> factory
                = new CustomWebApplicationFactory<Startup>();
            var client = factory.CreateClient();

            // Act
            var getResponse = await client.GetAsync("/Products/Details/4");

            // Assert
            getResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}
