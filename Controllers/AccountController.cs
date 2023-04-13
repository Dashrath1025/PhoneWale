using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shopify.Models;
using Shopify.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopify.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]

        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Name=model.Name,
                    UserName = model.Email,
                    Email = model.Email,
                    City=model.City
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Login(LoginViewModel model, string returnurl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.IsRemeber, false);

                if (result.Succeeded)
                {
                    var user = await userManager.FindByEmailAsync(model.Email);
                    var roles = await userManager.GetRolesAsync(user);
                    var loginTime = DateTime.Now;
                    HttpContext.Session.SetString("UserId", user.Id);
                    HttpContext.Session.SetString("LoginTime", loginTime.ToString());

                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "UserLogin");
                    }
                }

                ModelState.AddModelError(string.Empty, "UserName or pasword is Invalid");


            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Visitor");
        }

        [AllowAnonymous]
        [HttpGet]

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
