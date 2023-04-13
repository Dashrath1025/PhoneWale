using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shopify.ViewModel
{
    public class RoleViewModel
    {
        public int RoleId { get; set; }

        [Required]
        [Display(Name ="Role Name")]

        public string RoleName { get; set; }


    }
}
