/*
 *      @Author: yaile
 */

using System.ComponentModel.DataAnnotations;

namespace ITrack.Data.Entities
{
    public class Status
    {
        [Key]
        public int StatusId { get; set; }

        [Required(ErrorMessage = "Enter status name")]
        [StringLength(50, ErrorMessage = "The status name cannot exceed 50 characters")]
        public string StatusName { get; set; } = null!;

        public virtual ICollection<PlanStage> PlanStages { get; set; } = new List<PlanStage>();
    }
}
