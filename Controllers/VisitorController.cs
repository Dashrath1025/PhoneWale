using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Shopify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopify.Controllers
{
    public class VisitorController : Controller
    {
        private IWebHostEnvironment _hostingEnvironment;
        private ICategory _categoryRepository;
        private IProduct _productRepository;
        private ISubCategory _subCategoryRepository;
        private IUserLike _userlike;
        public VisitorController(ICategory categoryRepository, IWebHostEnvironment hostingEnvironment, IProduct productRepository, ISubCategory subCategoryRepository)
        {
            _categoryRepository = categoryRepository;
            _hostingEnvironment = hostingEnvironment;
            _productRepository = productRepository;
            _subCategoryRepository = subCategoryRepository;
        }

        public IActionResult Index()
        {
            ViewBag.Layout = "~/Views/Shared/Visitor.cshtml";
            ViewBag.Category = _categoryRepository.GetAllCategory();
            ViewData["PageTitle"] = "Product List";

            var result =
                from p in _productRepository.GetAllProduct()
                join c in _categoryRepository.GetAllCategory()
                on p.CatId equals c.Id
                join s in _subCategoryRepository.GetAllSubCategory() on p.subId equals s.SId
                select new {
                    p.Id,
                    p.Name,
                    p.Qty,
                    p.Rate,
                    p.Profile,
                    p.IsActive,
                    p.CatId,
                    Category = c.Name,
                    p.Brand,
                    p.Gen,
                    p.no_of_sim,
                    p.os_version,
                    p.RAM,
                    p.ROM,
                    p.subId,
                    SubCategory = s.Name,
                    p.color
                };

            List<ProductViewModel> products = new List<ProductViewModel>();
            foreach (var each in result)
            {
                products.Add(new ProductViewModel()
                {
                    Category = each.Category,
                    CatId = each.CatId,
                    Name = each.Name,
                    Id = each.Id,
                    Profile = each.Profile,
                    IsActive = each.IsActive,
                    Rate = each.Rate,
                    Qty = each.Qty,
                    Brand = each.Brand,
                    Gen = each.Gen,
                    no_of_sim = each.no_of_sim,
                    os_version = each.os_version,
                    RAM = each.RAM,
                    ROM = each.ROM,
                    color = each.color,
                    SubCategory = each.SubCategory
                });
            }
            return View(products);
        }


        public IActionResult Detail(int id)
        {
            var product = _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Qty = product.Qty,
                Rate = product.Rate,
                Profile = product.Profile,
                IsActive = product.IsActive,
                Brand = product.Brand,
                Gen = product.Gen,
                no_of_sim = product.no_of_sim,
                os_version = product.os_version,
                RAM = product.RAM,
                ROM = product.ROM,
                color = product.color,
               
            };

            // Pass the view model to the Detail view
            ViewBag.Layout = "~/Views/Shared/Visitor.cshtml";
            return View("Detail", viewModel);
        }

        public IActionResult SearchCategory(int? Cid)
        {
            var result =
                from p in _productRepository.GetAllProduct()
                join c in _categoryRepository.GetAllCategory()
                    on p.CatId equals c.Id
                join s in _subCategoryRepository.GetAllSubCategory()
                    on p.subId equals s.SId
                where Cid == null || p.CatId == Cid
                select new ProductViewModel
                {
                    Category = c.Name,
                    CatId = p.CatId,
                    subId = p.subId,
                    Name = p.Name,
                    Id = p.Id,
                    Profile = p.Profile,
                    IsActive = p.IsActive,
                    Rate = p.Rate,
                    Qty = p.Qty,
                    Brand = p.Brand,
                    Gen = p.Gen,
                    no_of_sim = p.no_of_sim,
                    os_version = p.os_version,
                    RAM = p.RAM,
                    ROM = p.ROM,
                    color = p.color,
                    SubCategory = s.Name
                };

            var productList = result.AsEnumerable();

            if (productList == null || !productList.Any())
            {
                TempData["error"] = $"!No Products available of this Category";
                return RedirectToAction("Index");
            }

            ViewBag.Layout = "~/Views/Shared/Visitor.cshtml";

            return View("search", productList);
        }


        

        [HttpGet]
        public IActionResult SearchBrand(string brand)
        {
            if (string.IsNullOrEmpty(brand))
            {
                TempData["error"] = "Please enter a brand name to search";
                return RedirectToAction("Index");
            }

            var result =
                from p in _productRepository.GetAllProduct()
                join c in _categoryRepository.GetAllCategory()
                    on p.CatId equals c.Id
                join s in _subCategoryRepository.GetAllSubCategory()
                    on p.subId equals s.SId
                where p.Brand.ToLower().Trim().Contains(brand.ToLower().Trim())
                select new ProductViewModel
                {
                    Category = c.Name,
                    CatId = p.CatId,
                    subId = p.subId,
                    Name = p.Name,
                    Id = p.Id,
                    Profile = p.Profile,
                    IsActive = p.IsActive,
                    Rate = p.Rate,
                    Qty = p.Qty,
                    Brand = p.Brand,
                    Gen = p.Gen,
                    no_of_sim = p.no_of_sim,
                    os_version = p.os_version,
                    RAM = p.RAM,
                    ROM = p.ROM,
                    color = p.color,
                    SubCategory = s.Name
                };

            var productList = result.AsEnumerable();

            if (productList == null || !productList.Any())
            {
                TempData["error"] = $"No products available for {brand}";
                return RedirectToAction("Index");
            }

            ViewBag.Layout = "~/Views/Shared/Visitor.cshtml";

            return View("search", productList);
        }

       


    }
}
