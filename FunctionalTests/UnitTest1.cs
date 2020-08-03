using Ganges;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace FunctionalTests
{
    [TestClass]
    public class UnitTest1
    {

        private WebApplicationFactory<Startup> _factory;

        
        public UnitTest1()
        {
            _factory = new WebApplicationFactory<Startup>();
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/products");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
