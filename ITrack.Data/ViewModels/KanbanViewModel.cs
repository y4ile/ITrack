/*
 *      @Author: yaile
 */

using ITrack.Data.Entities;

namespace ITrack.Data.ViewModels;

public class KanbanViewModel
{
    public string UserId { get; set; }
    public int StudyPlanId { get; set; }
    public string StudyPlanName { get; set; }
    public IEnumerable<Status> Statuses { get; set; }
    public Dictionary<int, List<PlanStage>> StagesByStatus { get; set; }
}