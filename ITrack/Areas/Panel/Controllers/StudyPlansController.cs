/*
 *      @Author: yaile
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ITrack.Data.Entities;
using ITrack.Data.ViewModels;
using ITrack.Service.Interfaces;

namespace ITrack.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Policy = "Mentoring")]
    public class StudyPlansController : Controller
    {
        private readonly IPlanStageService _stagesService;
        private readonly IStudyPlanService _plansService;
        private readonly IDirectionService _directionService;
        private readonly IUserService _userService;
        private readonly IStatusService _statusService;

        public StudyPlansController(IPlanStageService stagesService, IStudyPlanService plansService, IDirectionService directionService, IUserService userService, IStatusService statusService)
        {
            _plansService = plansService;
            _userService = userService;
            _directionService = directionService;
            _stagesService = stagesService;
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

            var response = await _plansService.GetAll();
            if (response.StatusCode == Data.Enum.StatusCode.OK)
            {
                var studyPlans = response.Data;
                var count = studyPlans.Count();

                var items = studyPlans.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var model = new StudyPlansListViewModel
                {
                    StudyPlans = items,
                    ViewType = viewType,
                    PageViewModel = new PageViewModel(count, page, pageSize)
                };

                return View(model);
            }

            return Problem(statusCode: (int)response.StatusCode);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateUsersAndDirections();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudyPlan model)
        {
            if (ModelState.IsValid)
            {
                var response = await _plansService.Create(model);
                if (response.StatusCode == Data.Enum.StatusCode.OK)
                    return RedirectToAction(nameof(Index));
                
                ModelState.AddModelError("", "Status code: " + response.StatusCode.ToString());
                return View(model);
            }
            await PopulateUsersAndDirections();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _plansService.Delete(id);
            if (response.StatusCode == Data.Enum.StatusCode.OK)
                return RedirectToAction(nameof(Index));
            
            return NotFound();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStage(int id, int planId)
        {
            var response = await _stagesService.Delete(id);
            if (response.StatusCode == Data.Enum.StatusCode.OK)
                return RedirectToAction(nameof(Edit), new { id = planId });
            
            return NotFound();
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _plansService.Get(id);
            if (response.StatusCode != Data.Enum.StatusCode.OK)
                return NotFound();

            var directionsResponse = await _directionService.GetAll();
            if (directionsResponse.StatusCode != Data.Enum.StatusCode.OK)
                return Problem(statusCode: (int)directionsResponse.StatusCode);
            
            var users = _userService.GetAll();

            var model = new EditStudyPlanViewModel
            {
                PlanID = response.Data.PlanID,
                PlanName = response.Data.PlanName,
                DirectionId = response.Data.DirectionId,
                Directions = directionsResponse.Data,
                UserId = response.Data.UserId,
                Users = users.ToList(),
                PlanStages = response.Data.PlanStages.ToList(),
            };
            
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditStudyPlanViewModel model)
        {
            if (id != model.PlanID)
                return NotFound();

            if (ModelState.IsValid)
            {
                var updatedPlan = new StudyPlan
                {
                    PlanID = model.PlanID,
                    PlanName = model.PlanName,
                    DirectionId = model.DirectionId,
                    UserId = model.UserId
                };
                
                var response = await _plansService.Edit(id, updatedPlan);
                if (response.StatusCode == Data.Enum.StatusCode.OK)
                    return RedirectToAction(nameof(Index));
                
                ModelState.AddModelError("", "Status code: " + response.StatusCode.ToString());
                return View(model);
            }
            
            var directionsResponse = await _directionService.GetAll();
            var usersResponse = _userService.GetAll();
            var plansResponse = await _plansService.Get(id);

            model.Directions = directionsResponse.Data;
            model.Users = usersResponse.ToList();
            model.PlanStages = plansResponse.Data.PlanStages.ToList();

            return View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> EditStage(int id)
        {
            var response = await _stagesService.Get(id);
            if (response.StatusCode != Data.Enum.StatusCode.OK)
                return NotFound();

            var statusesResponse = await _statusService.GetAll();
            if (statusesResponse.StatusCode != Data.Enum.StatusCode.OK)
                return Problem(statusCode: (int)statusesResponse.StatusCode);

            var model = new EditPlanStageViewModel
            {
                StageID = response.Data.StageID,
                StageName = response.Data.StageName,
                PlanId = response.Data.PlanId,
                StatusId = response.Data.StatusId,
                Statuses = statusesResponse.Data.ToList(),
                Priority = response.Data.Priority,
                OpenDate = response.Data.OpenDate.ToDateTime(TimeOnly.MinValue),
                CloseDate = response.Data.CloseDate != null ? response.Data.CloseDate.Value.ToDateTime(TimeOnly.MinValue) : null,
            };
            
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStage(int id, EditPlanStageViewModel model)
        {
            if (id != model.StageID)
                return NotFound();
            
            var stageResponse = await _stagesService.Get(id);
            if (stageResponse.StatusCode != Data.Enum.StatusCode.OK)
                ModelState.AddModelError("", "Status code: " + stageResponse.StatusCode.ToString());

            if (ModelState.IsValid)
            {
                var updatedStage = new PlanStage
                {
                    StageID = model.StageID,
                    StageName = model.StageName,
                    PlanId = model.PlanId,
                    StatusId = model.StatusId,
                    Priority = model.Priority,
                    OpenDate = DateOnly.FromDateTime(model.OpenDate),
                    CloseDate = model.CloseDate != null ? DateOnly.FromDateTime((DateTime)model.CloseDate) : null
                };
                
                var response = await _stagesService.Edit(id, updatedStage);
                if (response.StatusCode == Data.Enum.StatusCode.OK)
                    return RedirectToAction("Edit", new { id = stageResponse.Data.PlanId });
                
                ModelState.AddModelError("", "Status code: " + response.StatusCode.ToString());
                return View(model);
            }
            
            var statusesResponse = await _statusService.GetAll();
            model.Statuses = statusesResponse.Data.ToList();

            return View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> AddStage(int planId)
        {
            var statusesResponse = await _statusService.GetAll();
            if (statusesResponse.StatusCode != Data.Enum.StatusCode.OK)
                return Problem(statusCode: (int)statusesResponse.StatusCode);
            
            var model = new EditPlanStageViewModel
            {
                PlanId = planId,
                Priority = 0,
                OpenDate = DateTime.Now,
                Statuses = statusesResponse.Data.ToList()
            };
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStage(EditPlanStageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var planStage = new PlanStage
                {
                    StageName = model.StageName,
                    PlanId = model.PlanId,
                    StatusId = model.StatusId,
                    Priority = model.Priority,
                    OpenDate = DateOnly.FromDateTime(model.OpenDate),
                    CloseDate = model.CloseDate != null ? DateOnly.FromDateTime((DateTime)model.CloseDate) : null
                };

                var response = await _stagesService.Create(planStage);
                if (response.StatusCode == Data.Enum.StatusCode.OK)
                    return RedirectToAction("Edit", new { id = model.PlanId });
                
                ModelState.AddModelError("", response.StatusCode.ToString());
            }

            var statusesResponse = await _statusService.GetAll();
            model.Statuses = statusesResponse.Data;
            return View(model);
        }
        
        private async Task PopulateUsersAndDirections()
        {
            var users = _userService.GetAll();
            ViewBag.Users = users.ToList();

            var response = await _directionService.GetAll();
            if (response.StatusCode == Data.Enum.StatusCode.OK)
            {
                var directions = response.Data.ToList();
                ViewBag.Directions = directions;
            }
        }
    }
}
