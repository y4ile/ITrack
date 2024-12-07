/*
 *      @Author: yaile
 */

using ITrack.Data.Entities;

namespace ITrack.Data.ViewModels
{
    public class DirectionListViewModel
    {
        public IEnumerable<DevelopmentDirection> Directions { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public string ViewType { get; set; }
    }
}
