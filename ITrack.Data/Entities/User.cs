/*
 *      @Author: yaile
 */

using Microsoft.AspNetCore.Identity;

namespace ITrack.Data.Entities
{
    public class User : IdentityUser
    {
        public virtual ICollection<StudyPlan> StudyPlans { get; set; } = new List<StudyPlan>();
    }
}
