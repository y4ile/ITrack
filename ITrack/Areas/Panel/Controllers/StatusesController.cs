/*
 *      @Author: yaile
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ITrack.Data.Roles;
using ITrack.Data.Entities;
using ITrack.Data.ViewModels;
using ITrack.Service.Interfaces;

namespace ITrack.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Roles = RoleNames.Admin)]
    public class StatusesController : Controller
    {
        private readonly IStatusService _statusService;

        public StatusesController(IStatusService statusService)
        {
            _statusService = statusService;
        }

        [HttpGet]
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

            var response = await _statusService.GetAll();
            if (response.StatusCode == Data.Enum.StatusCode.OK)
            {
                var statuses = response.Data;

                var count = statuses.Count();
                var items = statuses.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var model = new StatusListViewModel
                {
                    Statuses = items,
                    PageViewModel = new PageViewModel(count, page, pageSize),
                    ViewType = viewType
                };

                return View(model);
            }

            return Problem(statusCode: (int)response.StatusCode);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Status model)
        {
            if (ModelState.IsValid)
            {
                var response = await _statusService.Create(model);
                if (response.StatusCode == Data.Enum.StatusCode.OK)
                    return RedirectToAction(nameof(Index));
                
                ModelState.AddModelError("", "Status code: " + response.StatusCode);
                return View(model);
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _statusService.Get(id);
            if (response.StatusCode == Data.Enum.StatusCode.OK)
                return View(response.Data);

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Status model)
        {
            if (ModelState.IsValid)
            {
                var response = await _statusService.Edit(id, model);
                if (response.StatusCode == Data.Enum.StatusCode.OK)
                    return RedirectToAction(nameof(Index));
                
                ModelState.AddModelError("", "Status code: " + response.StatusCode.ToString());
                return View(model);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _statusService.Delete(id);
            if (response.StatusCode == Data.Enum.StatusCode.OK)
                return RedirectToAction(nameof(Index));
            
            return NotFound();
        }
    }
}
