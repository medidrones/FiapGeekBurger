using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeekBurger.StoreCatalog.Contract
{
    public class Product
    {
        public string ProductId { get; set; }
        public IEnumerable<Item> Items { get; set; }

        public static IEnumerable<Product> GetProducts(string json)
        {
            return JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
        }
    }
}
