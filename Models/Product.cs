using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shopify.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product Name should not empty")]
        [MaxLength(100, ErrorMessage = "product name should  not more than 100")]
        public string Name { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Qty should be more than 0")]
        public int Qty { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Rate should be more than 0")]
        public int Rate { get; set; }
        public string Profile { get; set; }

        public bool IsActive { get; set; } = true;
        public int CatId { get; set; }

        public int subId { get; set; }


        [Required(ErrorMessage = "Brand Name should not empty")]
        [MaxLength(100, ErrorMessage = "Brand name should  not more than 100")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Gen Name should not empty")]
        public string Gen { get; set; }

        [Required(ErrorMessage = "color should not be empty")]
        public string color { get; set; }

        [Required(ErrorMessage ="No of sim should not be empty")]
        public int no_of_sim { get; set; }

        [Required]
        [Range(1,13,ErrorMessage ="OS version between 1 to 13 only")]
        public int os_version { get; set; }

        [Required]
        [Range(0,32,ErrorMessage ="Ram should not be more than 16")]
        public int RAM { get; set; }

        [Required]
        [Range(0, Int32.MaxValue, ErrorMessage = "Ram should not be more than 16")]
        public int ROM { get; set; }


    }
}
