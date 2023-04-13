using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shopify.Models
{
    public class SubCategory
    {

        [Key]
        public int SId { get; set; }

        [Required(ErrorMessage ="Subcategory name should not empty")]

        [MaxLength(50,ErrorMessage ="Subcategory Name should not more than 50")]
        public string Name { get; set; }
        public int Cid { get; set; }

    }
}
