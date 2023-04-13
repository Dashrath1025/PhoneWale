using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopify.Models
{
    public interface ICategory
    {
        Category GetCategoryId(int id);

        IEnumerable<Category> GetAllCategory();

        bool AddCategory(Category cat);

        Category GetCategoryByName(string name);
        void EditCategory(Category category);
        bool DeleteCategory(Category category);

    }
}
