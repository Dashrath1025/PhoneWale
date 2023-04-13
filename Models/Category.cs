using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shopify.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category should not be empty")]
        [MaxLength(50,ErrorMessage ="Category name should be more tgan 50")]
        public  string Name { get; set; }

    }
}
