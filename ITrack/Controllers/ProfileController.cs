/*
 *       @Author: yaile
 */

using ITrack.Data.Entities;
using ITrack.Data.ViewModels;
using ITrack.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITrack.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly IStudyPlanService _studyPlanService;
    
    public ProfileController(IStudyPlanService studyPlanService)
    {
        _studyPlanService = studyPlanService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Challenge();

        var studyPlansResponse = await _studyPlanService.GetAll();
        if (studyPlansResponse.StatusCode != Data.Enum.StatusCode.OK)
            return Problem(statusCode: (int)studyPlansResponse.StatusCode);
        
        var studyPlans = studyPlansResponse.Data.Where(sp => sp.UserId == userId);

        var completedPlanStages = new List<PlanStage>();
        foreach (var plan in studyPlans)
        {
            foreach (var stage in plan.PlanStages)
            {
                if (stage.CloseDate != null)
                    completedPlanStages.Add(stage);
            }
        }

        completedPlanStages = completedPlanStages.TakeLast(4).ToList();
        var model = new TimelineViewModel
        {
            PlanStages = completedPlanStages
        };
        
        return View(model);
    }
}