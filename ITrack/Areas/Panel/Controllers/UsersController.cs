/*
 *      @Author: yaile
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ITrack.Data.Roles;
using ITrack.Data.ViewModels;
using ITrack.Service.Interfaces;

namespace ITrack.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Roles = RoleNames.Admin)]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UsersController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 12, string viewType = null)
        {
            if (string.IsNullOrEmpty(viewType))
            {
                if (Request.Cookies.ContainsKey("viewType"))
                    viewType = Request.Cookies["viewType"];
                else
                    viewType = "cards";
            }
            else
            {
                Response.Cookies.Append("viewType", viewType, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                });
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = _userService.GetAll().Where(u => u.Id != currentUserId);
            var users = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var totalUsers = query.Count();

            var model = new UserListViewModel
            {
                Users = users,
                ViewType = viewType,
                PageViewModel = new PageViewModel(totalUsers, page, pageSize)
            };

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound();

            var userRoles = await _userService.GetUserRoles(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                SelectedRole = userRoles.FirstOrDefault(),
                Roles = _roleService.GetAll().ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            model.Roles = _roleService.GetAll().ToList();
            if (ModelState.IsValid)
            {
                var response = await _userService.UpdateUser(model);
                if (response.StatusCode == Data.Enum.StatusCode.OK)
                    return RedirectToAction(nameof(Index));
                
                ModelState.AddModelError("", "Status code: " + response.StatusCode.ToString());
                return View(model);
            }
            
            model.Roles = _roleService.GetAll().ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _userService.DeleteUser(id);
            if (response.StatusCode == Data.Enum.StatusCode.OK)
                return RedirectToAction(nameof(Index));
            
            return Problem(statusCode: (int)response.StatusCode);
        }
    }
}
