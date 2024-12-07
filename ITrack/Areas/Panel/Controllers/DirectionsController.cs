/*
 *      @Author: yaile
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ITrack.Data.Roles;
using ITrack.Data.Entities;
using ITrack.Data.ViewModels;

using ITrack.Service.Interfaces;

namespace ITrack.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Roles = RoleNames.Admin)]
    public class DirectionsController : Controller
    {
        private readonly IDirectionService _directionService;

        public DirectionsController(IDirectionService directionService)
        {
            _directionService = directionService;
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

            var response = await _directionService.GetAll();
            if (response.StatusCode == Data.Enum.StatusCode.OK)
            {
                var directions = response.Data;
                var count = directions.Count();
                var items = directions.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var model = new DirectionListViewModel
                {
                    Directions = directions,
                    ViewType = viewType,
                    PageViewModel = new PageViewModel(count, page, pageSize)
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
        public async Task<IActionResult> Create(DevelopmentDirection model)
        {
            if (ModelState.IsValid)
            {
                var response = await _directionService.Create(model);
                if (response.StatusCode == Data.Enum.StatusCode.OK)
                    return RedirectToAction(nameof(Index));
                
                ModelState.AddModelError("", "Status code: " + response.StatusCode.ToString());
                return View(model);
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _directionService.Get(id);
            if (response.StatusCode == Data.Enum.StatusCode.OK)
                return View(response.Data);

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DevelopmentDirection model)
        {
            if (ModelState.IsValid)
            {
                var response = await _directionService.Edit(id, model);
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
            var response = await _directionService.Delete(id);
            if (response.StatusCode == Data.Enum.StatusCode.OK)
                return RedirectToAction(nameof(Index));
            
            return NotFound();
        }
    }
}
