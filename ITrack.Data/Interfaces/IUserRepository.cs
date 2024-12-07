/*
 *      @Author: yaile
 */

using ITrack.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace ITrack.Data.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<User> GetAll();
        Task<User> FindById(string id);
        Task<User> FindByEmail(string email);
        Task<User> FindByName(string userName);
        Task<IdentityResult> Create(User user, string password);
        Task<IdentityResult> Update(User user);
        Task<IdentityResult> Delete(User user);
        Task<IList<string>> GetRoles(User user);
        Task<IdentityResult> AddToRole(User user, string role);
        Task<IdentityResult> RemoveFromRoles(User user, IEnumerable<string> roles);
        Task<SignInResult> PasswordSignIn(string userName, string password, bool isPersistent, bool lockoutOnFailure);
        Task SignIn(User user, bool isPersistent);
        Task SignOut();
    }
}
