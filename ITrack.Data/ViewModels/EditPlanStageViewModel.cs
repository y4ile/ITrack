/*
 *      @Author: yaile
 */

using System.ComponentModel.DataAnnotations;
using ITrack.Data.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ITrack.Data.ViewModels;

public class EditPlanStageViewModel
{
    public int StageID { get; set; }

    [Required]
    [StringLength(100)]
    public string StageName { get; set; }

    [Required]
    public int PlanId { get; set; }

    [Required]
    public int StatusId { get; set; }
    
    [ValidateNever]
    public IEnumerable<Status> Statuses { get; set; }

    [Required]
    public int Priority { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime OpenDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime? CloseDate { get; set; }
}