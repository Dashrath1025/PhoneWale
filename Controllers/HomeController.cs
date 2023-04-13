using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shopify.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Shopify.Controllers
{
    public class HomeController : Controller
    {
        

        private IWebHostEnvironment _hostingEnvironment;
        private ICategory _categoryRepository;
        private IProduct _productRepository;
        private ISubCategory _subCategoryRepository;
        public HomeController(ICategory categoryRepository, IWebHostEnvironment hostingEnvironment, IProduct productRepository,ISubCategory subCategoryRepository)
        {
            _categoryRepository = categoryRepository;
            _hostingEnvironment = hostingEnvironment;
            _productRepository = productRepository;
            _subCategoryRepository = subCategoryRepository;
        }


        public IActionResult Index()
        {
            ViewData["PageTitle"] = "Product List";

            var result =
                from p in _productRepository.GetAllProduct()
                join c in _categoryRepository.GetAllCategory() 
                on p.CatId equals c.Id
                join s in _subCategoryRepository.GetAllSubCategory() on p.subId equals s.SId
                select new
                {
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
                    SubCategory= s.Name,
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
                    Brand=each.Brand,
                    Gen=each.Gen,
                    no_of_sim=each.no_of_sim,
                    os_version=each.os_version,
                    RAM=each.RAM,
                    ROM=each.ROM,
                    color=each.color,
                    SubCategory=each.SubCategory
                });
            }
            return View(products);
        }


        public IActionResult Edit(int id)
        {
            Product p = _productRepository.GetProductById(id);
            ProductViewModel productViewModel = new ProductViewModel()
            {
                Name = p.Name,
                Qty = p.Qty,
                Rate = p.Rate,
                CatId = p.CatId,
                IsActive = p.IsActive,
                Profile = p.Profile,
                Id = p.Id,
                subId=p.subId,
                Brand=p.Brand,
                Gen=p.Gen,
                color=p.color,
                no_of_sim=p.no_of_sim,
                os_version=p.os_version,
                RAM=p.RAM,
                ROM=p.ROM
                
            };
            ViewBag.Category = _categoryRepository.GetAllCategory();
            ViewBag.SubCategory = _subCategoryRepository.GetAllSubCategory();
            return View("AddProduct", productViewModel);
        }

        public IActionResult CheckInsertUnique(string name, int catId)
        {
            bool value = _productRepository.CheckInsertUnique(name, catId);
            return Json(value);
        }
        public IActionResult CheckUpdateUnique(string name, int catId, int prodId)
        {
            bool value = _productRepository.CheckUpdateUnique(name, catId, prodId);
            return Json(value);
        }

        public IActionResult Delete(int id)
        {
            Product p = _productRepository.GetProductById(id);
            TempData["SuccessMessage"] = " Product Deleted Sucessfully.";
            _productRepository.DeleteProduct(p);
             
            return RedirectToAction("Index");
        }

        public IActionResult AddProduct()
        {
            ViewBag.Category = _categoryRepository.GetAllCategory();
            ViewBag.SubCategory = _subCategoryRepository.GetAllSubCategory();
            return View(new ProductViewModel());
        }

        public IActionResult Save(ProductViewModel p)
        {
            string uniqueFileName = string.Empty;
            if (p.Image != null)
            {
                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + p.Image.FileName;
                string uploadFilePath = Path.Combine(uploadFolder, uniqueFileName);
                p.Image.CopyTo(new FileStream(uploadFilePath, FileMode.Create));
                p.Profile = uniqueFileName;
            }

            if (p.Id > 0)
            {
                if (!_productRepository.UpdateProduct(p))
                {
                    ViewBag.SubCategory = _subCategoryRepository.GetAllSubCategory();
                    ViewBag.Category = _categoryRepository.GetAllCategory();
                    return View("AddProduct", p);
                }
            }
            else
            {
                if (!_productRepository.AddProduct(p))
                {
                    ViewBag.SubCategory = _subCategoryRepository.GetAllSubCategory();
                    ViewBag.Category = _categoryRepository.GetAllCategory();
                    return View("AddProduct", p);
                }
            }
            return RedirectToAction("Index");
        }



        public IActionResult Privacy()
        {
            return View();
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
