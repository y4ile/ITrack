/*
 *      @Author: yaile
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITrack.Data.Entities
{
    public class PlanStage
    {
        [Key]
        public int StageID { get; set; }

        [Required]
        [StringLength(100)]
        public string StageName { get; set; } = null!;

        [ForeignKey("StudyPlan")]
        public int PlanId { get; set; }

        [ForeignKey("Status")]
        public int StatusId { get; set; }

        [Required]
        public int Priority { get; set; }

        [Required]
        public DateOnly OpenDate { get; set; }

        public DateOnly? CloseDate { get; set; }

        public virtual StudyPlan Plan { get; set; } = null!;

        public virtual Status Status { get; set; } = null!;
    }
}
