using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopify.Models
{
    public class SqlSubCategoryRepository: ISubCategory
    {
        private ShopifyDbContext _subcategory;
        public SqlSubCategoryRepository(ShopifyDbContext context)
        {
            _subcategory = context;
        }

        public bool AddSubCategory(SubCategory subcat)
        {

            if (CheckInsertUnique(subcat.Cid, subcat.Name))
            {

                _subcategory.Add(subcat);
                _subcategory.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckInsertUnique(int Cid, string name)
        {
            SubCategory isDuplicate = _subcategory.SubCategories.FirstOrDefault(each => each.Cid == Cid && name.ToLower() == each.Name.ToLower());
            return isDuplicate == null ? true : false;
        }

        public IEnumerable<SubCategory> GetAllSubCategory()
        {
            return _subcategory.SubCategories;
        }

        //public IEnumerable<SubCategory> GetAllProd(int cid)
        //{
        //    return _subcategory.SubCategories.FirstOrDefault(each=>each.Cid==cid)
        //}

        public SubCategory GetSubCategoryId(int id)
        {
            return _subcategory.SubCategories.SingleOrDefault(each => each.SId == id);
        }

        public bool DeleteSubCategory(SubCategory cat)
        {
            SubCategory isDuplicate = _subcategory.SubCategories.FirstOrDefault(each => each.SId == cat.SId);
            if (isDuplicate != null)
            {
                _subcategory.Remove(cat);
                _subcategory.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EditSubCategory(SubCategory s)
        {
            if (CheckUpdateUnique(s.SId, s.Cid, s.Name))
            {
                _subcategory.SubCategories.Update(s);
                _subcategory.SaveChanges();
                return true;
            }
            return false;
        }

        public bool CheckUpdateUnique(int Sid, int Cid, string name)
        {
            SubCategory sub = _subcategory.SubCategories.FirstOrDefault(each => each.Name.ToLower() == name.ToLower());
            if (sub == null)
                return true;
            sub = _subcategory.SubCategories.FirstOrDefault(each => each.Name.ToLower() == name.ToLower() && each.Cid == Cid && each.SId == Sid);
            if (sub != null)
                return true;
            bool flag = false;

            foreach (SubCategory s in _subcategory.SubCategories)
            {
                if (s.Name.ToLower() == name.ToLower() && s.Cid != Cid)
                {
                    flag = true;
                }
                else if (s.Name.ToLower() == name.ToLower() && s.Cid == Cid)
                {
                    return false;
                }
            }
            return flag;
        }

        public IEnumerable<SubCategory> GetAllProd()
        {
            throw new NotImplementedException();
        }
    }
}
