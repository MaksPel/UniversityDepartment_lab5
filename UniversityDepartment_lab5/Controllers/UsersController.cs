using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UniversityDepartment_lab5.Data;
using UniversityDepartment_lab5.ViewModels.Users;

namespace UniversityDepartment_lab5.Controllers
{
    /*[Authorize(Roles = "admin")]*/
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UniversityDbContext _context;

        public UsersController(RoleManager<IdentityRole> _roleManager, UserManager<IdentityUser> _userManager, UniversityDbContext context)
        {
            this._roleManager = _roleManager;
            this._userManager = _userManager;
            this._context = context;
        }
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.OrderBy(user => user.Id).ToList();
            List<UsersViewModel> usersViewModel = new List<UsersViewModel>();

            var rolesList = _roleManager.Roles.ToList();
            var userRoles = _context.UserRoles.ToList();

            foreach (var user in users)
            {
                var userRole = userRoles.FirstOrDefault(ur => ur.UserId == user.Id);
                var roleName = userRole != null
                    ? rolesList.FirstOrDefault(r => r.Id == userRole.RoleId)?.Name
                : "No Role";

                usersViewModel.Add(new UsersViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    RoleName = roleName
                });
            }

            return View(usersViewModel);
        }

        public IActionResult Create()
        {
            var allRoles = _roleManager.Roles.ToList();
            CreateUserViewModel user = new();

            ViewData["UserRole"] = new SelectList(allRoles, "Name", "Name");

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new()
                {
                    Email = model.Email,
                    UserName = model.UserName
                };
                var result = await _userManager.CreateAsync(user, model.Password);
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

                var role = model.UserRole;
                if (role.Length > 0)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }
            return View(model);
        }

        // GET: IdentityUser/Details/{id}
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var viewModel = new IdentityUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };

            return View(viewModel);
        }

        // GET: IdentityUser/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var viewModel = new IdentityUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };

            return View(viewModel);
        }

        // POST: IdentityUser/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, IdentityUserViewModel viewModel)
        {
            if (id != viewModel.Id) return BadRequest();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.UserName = viewModel.UserName;
            user.Email = viewModel.Email;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Details), new { id = user.Id });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(viewModel);
        }

        // GET: IdentityUser/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var viewModel = new IdentityUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };

            return View(viewModel);
        }

        // POST: IdentityUser/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(new IdentityUserViewModel { Id = id });
        }
    }
}
