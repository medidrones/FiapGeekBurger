using System;
using System.Collections.Generic;
using System.Linq;
using GeekBurger.StoreCatalog.Contract;
using GeekBurger.StoreCatalog.Core.Interfaces;
using GeekBurger.StoreCatalog.Infra.Interfaces;

namespace GeekBurger.StoreCatalog.Core
{
    public class ProductCore : IProductCore
    {
        private readonly IRequestApi _requestApi;
        private readonly IRepository<ProductionAreas> _repository;

        public ProductCore(IRequestApi requestApi, IRepository<ProductionAreas> repository)
        {
            _requestApi = requestApi;
            _repository = repository;
        }

        public IEnumerable<Product> GetProductsFromUser(User user)
        {
            var restrictions = String.Join(",", user.Restrictions);
            var responseProducts = _requestApi.GetProducts(restrictions).GetAwaiter().GetResult();
            
            if (responseProducts.IsSuccessStatusCode)
            {
                var products = Product.GetProducts(responseProducts.Content.ReadAsStringAsync().Result);
                var productionAreas = _repository.GetAll();
                var result = productionAreas.Where(a => ContainsAny(a.Restrictions, user.Restrictions)).ToList();
                
                return products;
            }
            else
            {
                throw new Exception($"Status code error: {responseProducts.StatusCode}");
            }
        }

        private bool ContainsAny<T>(IEnumerable<T> Collection, IEnumerable<T> Values)
        {
            return Collection.Any(x => Values.Contains(x));
        }
    }
}
