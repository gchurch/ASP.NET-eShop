using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalTests
{
    public abstract class FunctionalTests<TStartup> where TStartup : class
    {
        public CustomWebApplicationFactory<TStartup> _factory = new CustomWebApplicationFactory<TStartup>();

        public StringContent SerializeObject(object objectToSerialize)
        {
            string productString = JsonConvert.SerializeObject(objectToSerialize);
            StringContent stringContent = new StringContent(productString, Encoding.UTF8, "application/json");
            return stringContent;
        }

        public async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            string responseString = await response.Content.ReadAsStringAsync();
            T result = JsonConvert.DeserializeObject<T>(responseString);
            return result;
        }

        public HttpClient CreateAuthorizedTestHttpClient()
        {
            HttpClient client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            "Test", options => { });
                });
            })
                .CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                }
            );

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Test");

            return client;
        }
    }
}
