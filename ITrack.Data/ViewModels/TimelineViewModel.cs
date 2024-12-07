/*
 *       @Author: yaile
 */

using ITrack.Data.Entities;

namespace ITrack.Data.ViewModels;

public class TimelineViewModel
{
    public IEnumerable<PlanStage> PlanStages { get; set; }
}