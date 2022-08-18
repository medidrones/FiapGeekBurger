using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GeekBurger.StoreCatalog.Infra.Interfaces;

namespace GeekBurger.StoreCatalog.Infra.Services
{
    public class RequestApi : IRequestApi
    {
        HttpClient _client;

        public RequestApi()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://geekburguer.azurewebsites.net/swagger/index.html")
            };
        }

        public Task<HttpResponseMessage> GetProductionAreas()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                return client.GetAsync("");
            }
        }

        public Task<HttpResponseMessage> GetProducts(string restrictions)
        {
            using (var client = new HttpClient())
            {
                return _client.GetAsync($"api/products/byrestrictions/{restrictions}");
            }
        }
    }
}
