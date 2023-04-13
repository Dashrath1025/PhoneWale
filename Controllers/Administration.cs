using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles ="Admin")]
    public class Administration : Controller
    {

        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public Administration(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = model.RoleName
                };

            IdentityResult result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                else
                {
                    foreach (var each in result.Errors)
                        ModelState.AddModelError("", each.Description);
                }
            }
            return View(model);
        }

        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]

        public async Task<IActionResult> EditRole(string id)
        {
            var role = roleManager.Roles.FirstOrDefault(each => each.Id == id);
            EditRoleViewModel model = new EditRoleViewModel()
            {
                RoleName = role.Name,
                RoleId = role.Id
            };

            foreach(var user in userManager.Users)
            {
                if(await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.UserName.Add(user.UserName);
                }
            }
            return View(model);
        }

        

        public IActionResult EditRole()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> EditRole(EditRoleViewModel models)
        {
            var role = roleManager.Roles.FirstOrDefault(each => each.Id == models.RoleId);
            role.Name = models.RoleName;
            var result = await roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles");
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(models);
        }

       [HttpGet]

       public async Task<IActionResult> EditUserInRole(string roleId)
        {
            ViewBag.RoleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {roleId} can not found";
                return View("NotFound");
            }
            List<UserRoleViewModel> users = new List<UserRoleViewModel>();
            foreach(var user in userManager.Users)
            {
                UserRoleViewModel userRoleViewModel = new UserRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName

                };
                if(await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                users.Add(userRoleViewModel);
            }
            return View(users);
        }


        [HttpPost]

        public async Task<IActionResult> EditUserInRole(List<UserRoleViewModel> model,string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role Id with = {roleId} can not found";
                return View("NotFound");
            }
            for(int index=0; index < model.Count; index++)
            {
                var user = await userManager.FindByIdAsync(model[index].UserId);
                IdentityResult result = null;
                if (model[index].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if(!model[index].IsSelected && (await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
            }
            return RedirectToAction("EditRole", new { Id = roleId });
        }


    }
}
