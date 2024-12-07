/*
 *      @Author: yaile
 */

using System.ComponentModel.DataAnnotations;

namespace ITrack.Data.Entities
{
    public class DevelopmentDirection
    {
        [Key]
        public int DirectionID { get; set; }

        [Required]
        [StringLength(100)]
        public string DirectionName { get; set; } = null!;

        public virtual ICollection<StudyPlan> StudyPlans { get; set; } = new List<StudyPlan>();
    }
}
