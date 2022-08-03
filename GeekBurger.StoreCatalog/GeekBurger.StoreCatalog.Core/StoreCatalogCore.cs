using System.Net.Http;
using GeekBurger.StoreCatalog.Core.Interfaces;

namespace GeekBurger.StoreCatalog.Core
{
    public class StoreCatalogCore : IStoreCatalogCore
    {
        private string _url_1 = "http://www.youtube.com.br/";
        private string _url_2 = "http://www.terra.com.br/";

        public bool StatusServers()
        {
            try
            {
                var client = new HttpClient();
                var result_1 = client.GetAsync(_url_1).GetAwaiter().GetResult();
                var result_2 = client.GetAsync(_url_2).GetAwaiter().GetResult();
                return (result_1.IsSuccessStatusCode && result_2.IsSuccessStatusCode);
            }
            catch
            {
                return false;
            }
        }
    }
}
