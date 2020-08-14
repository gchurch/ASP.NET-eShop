using Ganges;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Threading.Tasks;
using System.Net;
using Ganges.ApplicationCore.Entities;
using Newtonsoft.Json;

namespace FunctionalTests
{

    // Useful information: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.1

    // These tests could also be described as end-to-end functional tests.

    // TODO: improve these functional tests.

    [TestClass]
    public class FunctionalTests
    {

        private CustomWebApplicationFactory<Startup> _factory;

        public FunctionalTests()
        {
            _factory = new CustomWebApplicationFactory<Startup>();
        }

        [DataTestMethod]
        [DataRow("/api/products")]
        [DataRow("/api/products/1")]
        [DataRow("/api/products/2")]
        [DataRow("/api/products/3")]
        public async Task TestEndpointsRespondWithOk(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task TestAPI_GetProducts()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/products");
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Product[]>(responseString);

            // Assert
            result.Length.ShouldBe(3);
            result[0].Id.ShouldBe(1);
            result[0].Title.ShouldBe("Toy");
            result[1].Id.ShouldBe(2);
            result[1].Title.ShouldBe("Book");
            result[2].Id.ShouldBe(3);
            result[2].Title.ShouldBe("Lamp");
        }

        [TestMethod]
        public async Task TestAPI_GetProduct()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/products/2");
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Product>(responseString);

            // Assert
            result.Id.ShouldBe(2);
            result.Title.ShouldBe("Book");
            result.Description.ShouldBe("Hard back");
            result.Seller.ShouldBe("Peter");
            result.Price.ShouldBe(25);
            result.Quantity.ShouldBe(4);
            result.ImageUrl.ShouldBe("book.png");
        }

        [TestMethod]
        public async Task Test()
        {
            // Arrange
            CustomWebApplicationFactory<Startup> factory
                = new CustomWebApplicationFactory<Startup>();
            var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/products");

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
