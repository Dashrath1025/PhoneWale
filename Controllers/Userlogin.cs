using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shopify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopify.Controllers
{
    [Authorize]
    public class Userlogin : Controller
    {
        private IWebHostEnvironment _hostingEnvironment;
        private ICategory _categoryRepository;
        private IProduct _productRepository;
        private ISubCategory _subCategoryRepository;
        private IUserLike _userLike;
        private IRecently _recently;
        //private readonly UserManager<ApplicationUser> userManager;
        //private readonly SignInManager<ApplicationUser> signInManager;

        public Userlogin(ICategory categoryRepository, IWebHostEnvironment hostingEnvironment, IProduct productRepository, ISubCategory subCategoryRepository,IUserLike userLike,IRecently recently)
        {
            _categoryRepository = categoryRepository;
            _hostingEnvironment = hostingEnvironment;
            _productRepository = productRepository;
            _subCategoryRepository = subCategoryRepository;
            _userLike = userLike;
            _recently = recently;
            //this.userManager = userManager;
            //this.signInManager = signInManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var userId = HttpContext.Session.GetString("UserId");
          
            ViewBag.UserId = userId;
            
            ViewBag.Category = _categoryRepository.GetAllCategory();

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
                    subId = each.subId,
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

        public IActionResult Details(int Pid)
        {
            var result =
                from p in _productRepository.GetAllProduct()
                join c in _categoryRepository.GetAllCategory()
                on p.CatId equals c.Id
                join s in _subCategoryRepository.GetAllSubCategory() on p.subId equals s.SId
                where p.Id == Pid
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

            var product = result.FirstOrDefault();

            if (product == null)
            {
                return NotFound();
            }


            ViewData["PageTitle"] = product.Name;

           
            return View("loginDetail", product);
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



           

            return View("logsearched", productList);
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
                TempData["error"] = $"No products found for Company {brand}.";
                return RedirectToAction("Index");
            }

           

            return View("logsearched", productList);
        }

        public IActionResult AddUserLike(int Pid, string Uid, string Pname)
        {
            UserLike likes = new UserLike() { PId = Pid, UId = Uid, PName = Pname };

            bool result = _userLike.AddUserLike(likes);

            if (result)
            {
                TempData["success"] = "Product Liked Successfully.";
            }
            else
            {
                TempData["error"] = "Error: Failed to Like Product.";
            }
            return Redirect("Index");
        }

        public IActionResult GetLikesByUserId()
        {
            ViewData["error"] = TempData["error"];
            ViewData["success"] = TempData["success"];
            var userId = HttpContext.Session.GetString("UserId");
            ViewBag.UserId = userId;
            var likes = _userLike.GetLikesByUserId(userId);

            return View("UserLike", likes);
        }


        public IActionResult RemoveUserLike(int Lid)
        {
          

         
            UserLike liked = _userLike.GetUserLikeById(Lid);
            _userLike.RemoveUserLike(liked);
            TempData["success"] = "Liked Product Removed Sucessfully.";
            return Redirect("GetLikesByUserId");
        }

        public IActionResult GetUserHistoryByUserId()
        {
            ViewData["ErrorMessage"] = TempData["ErrorMessage"];
            ViewData["SuccessMessage"] = TempData["SuccessMessage"];
            var userId = HttpContext.Session.GetString("UserId");
            ViewBag.UserId = userId;
            var history =  _recently.GetRecentByUserId(userId);

            return View("history", history);
        }


    }
}
