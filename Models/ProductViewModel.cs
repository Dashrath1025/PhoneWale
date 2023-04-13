using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Shopify.Models
{
    public class ProductViewModel:Product
    {
        public string Category { get; set; }

        public string SubCategory { get; set; }
        public IFormFile Image { get; set; }
    }
}
