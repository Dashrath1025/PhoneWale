using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopify.Models
{
    public interface IProduct
    {
        public Product GetProductById(int id);

        public IEnumerable<Product> GetAllProduct();
        public bool AddProduct(Product e);
        public bool UpdateProduct(Product e);
        bool DeleteProduct(Product e);

        public bool CheckUpdateUnique(string name, int catId, int prodId);

        public bool CheckInsertUnique(string name, int catId);
    }
}
