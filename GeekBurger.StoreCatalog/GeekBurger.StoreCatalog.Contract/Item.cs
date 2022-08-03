using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeekBurger.StoreCatalog.Contract
{
    public class Item
    {
        [JsonProperty("id")]
        public string ItemId { get; set; }
        [JsonProperty("ingredients")]
        public IEnumerable<string> Ingredients { get; set; }
    }
}
