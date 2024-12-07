/*
 *      @Author: yaile
 */

using ITrack.Data.Entities;

namespace ITrack.Data.ViewModels
{
    public class StudyPlansListViewModel
    {
        public IEnumerable<StudyPlan> StudyPlans { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public string ViewType { get; set; }
    }
}
