using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopify.Models
{
    public interface ISubCategory
    {
        SubCategory GetSubCategoryId(int id);
        IEnumerable<SubCategory> GetAllSubCategory();

        IEnumerable<SubCategory> GetAllProd();
        bool AddSubCategory(SubCategory subcat);


        bool DeleteSubCategory(SubCategory subcat);
        bool EditSubCategory(SubCategory subcat);

        public bool CheckUpdateUnique(int Sid, int Cid, string name);
    }
}
