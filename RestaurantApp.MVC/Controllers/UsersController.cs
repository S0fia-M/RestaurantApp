using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data.Models.Users;
using RestaurantApp.MVC.ViewModels.Users;
using System.Threading.Tasks;

namespace RestaurantApp.MVC.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<User> userManager;

        public UsersController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index() => View(await this.userManager.Users.ToListAsync());

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add() => View();

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(AddUserViewModel userViewModel)
        {
            if(ModelState.IsValid)
            {
                var user = new User { Email = userViewModel.Email, UserName = userViewModel.Email, BirthYear = userViewModel.BirthYear };
                var result = await this.userManager.CreateAsync(user, userViewModel.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(userViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var viewModel = new EditUserViewModel { Id = user.Id, Email = user.Email, BirthYear = user.BirthYear };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await this.userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.BirthYear = model.BirthYear;

                    var result = await this.userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await this.userManager.FindByIdAsync(id);
            if (user != null)
            {
                await this.userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }
    }
}
