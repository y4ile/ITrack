/*
 *      @Author: yaile
 */

using ITrack.Data.Entities;
using ITrack.Data.ViewModels;
using ITrack.Models;
using ITrack.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITrack.Controllers;

[Authorize]
[ApiController]
[Route("[controller]/[action]")]
public class MyPlansController : Controller
{
    private readonly IStudyPlanService _studyPlanService;
    private readonly IPlanStageService _planStageService;
    private readonly IStatusService _statusService;

    public MyPlansController(
        IStudyPlanService studyPlanService,
        IPlanStageService planStageService,
        IStatusService statusService)
    {
        _studyPlanService = studyPlanService;
        _planStageService = planStageService;
        _statusService = statusService;
    }
    
    public async Task<IActionResult> Index(int page = 1, int pageSize = 12)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Challenge();
        
        var response = await _studyPlanService.GetAll();
        if (response.StatusCode == Data.Enum.StatusCode.OK)
        {
            var studyPlans = response.Data.Where(sp => sp.UserId == userId);
            var count = studyPlans.Count();

            var items = studyPlans.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var model = new StudyPlansListViewModel
            {
                StudyPlans = items,
                ViewType = "cards",
                PageViewModel = new PageViewModel(count, page, pageSize)
            };

            return View(model);
        }

        return Problem(statusCode: (int)response.StatusCode);
    }
    
    public async Task<IActionResult> Kanban(int id)
    {
        // [INFO] Current user id
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Challenge();

        var plansResponse = await _studyPlanService.GetAll();
        var studyPlan = plansResponse.Data.FirstOrDefault(p => p.PlanID == id && p.UserId == userId);
        if (studyPlan == null)
        {
            ModelState.AddModelError("", "Study plans not found.");
            return View(new KanbanViewModel());
        }

        // [INFO] Getting all statuses
        var statusesResponse = await _statusService.GetAll();
        if (statusesResponse.StatusCode != Data.Enum.StatusCode.OK || !statusesResponse.Data.Any())
        {
            ModelState.AddModelError("", "Cannot load statuses.");
            return View(new KanbanViewModel());
        }
        var statuses = statusesResponse.Data.ToList();

        // [INFO] Grouping stages
        var stagesByStatus = new Dictionary<int, List<PlanStage>>();
        foreach (var status in statuses)
        {
            var stagesForStatus = studyPlan.PlanStages.Where(s => s.StatusId == status.StatusId).OrderBy(s => s.Priority).ToList();
            stagesByStatus[status.StatusId] = stagesForStatus;
        }

        var model = new KanbanViewModel
        {
            UserId = userId,
            StudyPlanId = studyPlan.PlanID,
            StudyPlanName = studyPlan.PlanName,
            Statuses = statuses,
            StagesByStatus = stagesByStatus
        };

        return View(model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStagePosition([FromBody] UpdatePositionModel model)
    {
        // [INFO] Getting selected stage
        var stageResponse = await _planStageService.Get(model.StageID);
        if (stageResponse.StatusCode != Data.Enum.StatusCode.OK || stageResponse.Data == null)
            return Json(new { success = false, message = "Stage not found" });

        var stage = stageResponse.Data;
        
        // [INFO] Getting status
        var statusResponse = await _statusService.Get(model.NewStatusID);
        if (statusResponse.StatusCode != Data.Enum.StatusCode.OK || statusResponse.Data == null)
            return Json(new { success = false, message = "Status not found" });

        // [INFO] Updating status
        stage.StatusId = statusResponse.Data.StatusId;
        stage.Status = statusResponse.Data;
        
        // [INFO] Handling close date
        if (stage.Status.StatusName.ToLower().Contains("done") ||
            stage.Status.StatusName.ToLower().Contains("finished") ||
            stage.Status.StatusName.ToLower().Contains("completed"))
            stage.CloseDate = DateOnly.FromDateTime(DateTime.Now);
        else
            stage.CloseDate = null;
        
        // [INFO] Recalculating priority
        for (int i = 0; i < model.OrderedStageIDs.Count; i++)
        {
            var id = model.OrderedStageIDs[i];
            var currStageResponse = await _planStageService.Get(id);
            if (currStageResponse.StatusCode == Data.Enum.StatusCode.OK && currStageResponse.Data != null)
            {
                var currStage = currStageResponse.Data;
                currStage.Priority = i;
                await _planStageService.Edit(currStage.StageID, currStage);
            }
        }

        return Json(new { success = true });
    }
}