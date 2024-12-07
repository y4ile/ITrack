/*
 *      @Author: yaile
 */

using ITrack.Data.Entities;

namespace ITrack.Data.ViewModels
{
    public class StatusListViewModel
    {
        public IEnumerable<Status> Statuses { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public string ViewType { get; set; }
    }
}
