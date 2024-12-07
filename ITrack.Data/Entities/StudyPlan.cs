/*
 *      @Author: yaile
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITrack.Data.Entities
{
    public class StudyPlan
    {
        [Key]
        public int PlanID { get; set; }

        [Required]
        [StringLength(100)]
        public string PlanName { get; set; } = null!;

        [Required]
        [ForeignKey("DevelopmentDirection")]
        public int DirectionId { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual DevelopmentDirection? Direction { get; set; }

        public virtual ICollection<PlanStage> PlanStages { get; set; } = new List<PlanStage>();

        public virtual User? User { get; set; }
    }
}
