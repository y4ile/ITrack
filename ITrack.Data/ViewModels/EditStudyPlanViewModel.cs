/*
 *      @Author: yaile
 */

using System.ComponentModel.DataAnnotations;
using ITrack.Data.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ITrack.Data.ViewModels;

public class EditStudyPlanViewModel
{
    public int PlanID { get; set; }

    [Required]
    [StringLength(100)]
    public string PlanName { get; set; }

    [Required]
    public int DirectionId { get; set; }
    [ValidateNever]
    public IEnumerable<DevelopmentDirection> Directions { get; set; } = null!;

    [Required]
    public string UserId { get; set; }
    [ValidateNever]
    public IEnumerable<User> Users { get; set; } = null!;
    [ValidateNever]
    public IEnumerable<PlanStage> PlanStages { get; set; } = null!;
}