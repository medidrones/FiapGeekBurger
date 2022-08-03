using System.Collections.Generic;

namespace GeekBurger.StoreCatalog.Contract
{
    public class ProductsByUserWrapper
    {
        private User user;
        public User User
        {
            get { return user; }
        }

        private IEnumerable<Product> products;
        public IEnumerable<Product> Products
        {
            get { return products; }
        }

        public ProductsByUserWrapper(User user, IEnumerable<Product> products)
        {
            Builder(user, products);
        }

        private void Builder(User user, IEnumerable<Product> products)
        {
            this.user = user;
            this.products = products;
        }
    }
}
