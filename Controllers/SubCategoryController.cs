using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopify.Models;

namespace Shopify.Controllers
{
    public class SubCategoryController : Controller
    {

        private ISubCategory _subCategory;
        private ICategory _category;
        private IProduct _product;
        public SubCategoryController(ISubCategory subCategoryRepository,ICategory category,IProduct product)
        {
            _subCategory = subCategoryRepository;
            _category = category;
            _product = product;
        }
        public IActionResult Index()
        {
            ViewData["SuccessMessage"] = TempData["SuccessMessage"];
            ViewData["ErrorMessage"] = TempData["ErrorMessage"];
            ViewBag.Category = _category.GetAllCategory();
            return View("Index",_subCategory.GetAllSubCategory());
        }

        public IActionResult AddSubCAtegory(string name,int Cid)
        {
            SubCategory Subcategory = new SubCategory() { Cid = Cid, Name = name };
            return Json(_subCategory.AddSubCategory(Subcategory));
        }

        public IActionResult FindId(int Sid)
        {
            return Json(_subCategory.GetSubCategoryId(Sid));
        }

        public IActionResult DeleteSubCategory(int Sid)
        {
            bool isProductPresent = _product.GetAllProduct().Any(p => p.subId == Sid);

            if (isProductPresent)
            {
                TempData["ErrorMessage"] = "It is not possible beacuse,product is present";
            return RedirectToAction("Index");
            }

            SubCategory subcategory = _subCategory.GetSubCategoryId(Sid);
            _subCategory.DeleteSubCategory(subcategory);
            TempData["SuccessMessage"] = "SubCategory Deleted Sucessfully.";
            return RedirectToAction("Index");
        }

        public IActionResult CheckUpdateUnique(int Sid, int Cid, string name)
        {
            bool value = _subCategory.CheckUpdateUnique(Sid, Cid, name);
            return Json(value);
        }

        public IActionResult Edit(int Sid, int cid, string name)
        {
            var subcategory = _subCategory.GetSubCategoryId(Sid);

            if (subcategory == null)
            {
                return NotFound();
            }

            subcategory.Name = name;
            subcategory.Cid = cid;

            if (_subCategory.EditSubCategory(subcategory))
            {
                return Json(new { success = true });
            }
            else
            {
                return BadRequest("Unable to update subcategory.");
            }
        }


    }
}
