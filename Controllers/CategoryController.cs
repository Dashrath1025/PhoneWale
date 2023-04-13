using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopify.Models;
using Microsoft.AspNetCore.Hosting;

namespace Shopify.Controllers
{
    public class CategoryController : Controller
    {
        private IWebHostEnvironment _hostingEnvironment;
        private ICategory _categoryRepository;
        private ISubCategory _subCategory;
        public CategoryController(ICategory categoryRepository, ISubCategory subCategory,IWebHostEnvironment hostingEnvironment)
        {
            _categoryRepository = categoryRepository;
            _hostingEnvironment = hostingEnvironment;
            _subCategory = subCategory;
        }
        public IActionResult Index()
        {
            ViewData["SuccessMessage"] = TempData["SuccessMessage"];
            ViewData["ErrorMessage"] = TempData["ErrorMessage"];
            return View(_categoryRepository.GetAllCategory());
        }

        public IActionResult AddCategory(string name)
        {
            Category category = new Category() { Name = name };
            return Json(_categoryRepository.AddCategory(category));
        }

        
        public IActionResult Edit(int Cid, string name)
        {
            var category = _categoryRepository.GetCategoryId(Cid);

            if (category == null)
            {
                return NotFound();
            }

            category.Name = name;
            _categoryRepository.EditCategory(category);

            return Json(new { success = true });
        }

        public IActionResult RemoveCategory(int cid)
        {

            bool isCategoryPresentInSubCategory = _subCategory.GetAllSubCategory().Any(sc => sc.Cid == cid);

            if (isCategoryPresentInSubCategory)
            {
                TempData["ErrorMessage"] = "It is not possible beacuse,subcategory is present";
                return RedirectToAction("Index");
            }

            Category category = _categoryRepository.GetCategoryId(cid);
            _categoryRepository.DeleteCategory(category);
            TempData["SuccessMessage"] = " Category Deleted Sucessfully.";
            return RedirectToAction("Index");
        }



    }
}
