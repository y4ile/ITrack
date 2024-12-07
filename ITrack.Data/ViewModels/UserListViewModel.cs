/*
 *      @Author: yaile
 */

using ITrack.Data.Entities;

namespace ITrack.Data.ViewModels
{
    public class UserListViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public string ViewType { get; set; }
    }
}
