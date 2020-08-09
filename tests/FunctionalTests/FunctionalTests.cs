using Ganges;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Threading.Tasks;
using System.Net;

namespace FunctionalTests
{

    // Useful information: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.1

    // These tests could also be described as end-to-end functional tests.

    // TODO: improve these functional tests.

    [TestClass]
    public class FunctionalTests
    {

        private WebApplicationFactory<Startup> _factory;

        public FunctionalTests()
        {
            _factory = new WebApplicationFactory<Startup>();
        }

        // Make sure that you have run 'npm start' in a terminal window
        // otherwise this test will fail.
        [DataTestMethod]
        [DataRow("/")]
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
    }
}
