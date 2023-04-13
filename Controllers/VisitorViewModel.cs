using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopify.Controllers
{
    internal class VisitorViewModel 
    {
        public object Products { get; set; }
        public string CurrentUser { get; set; }
    }
}
