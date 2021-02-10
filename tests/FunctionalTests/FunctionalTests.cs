using ApplicationCore.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
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
    }
}
