/*
 *      @Author: yaile
 */

using Microsoft.AspNetCore.Identity;
using ITrack.Data.Entities;
using ITrack.Data.Interfaces;

namespace ITrack.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        
        public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IQueryable<User> GetAll() => _userManager.Users;
        public Task<User> FindById(string id) => _userManager.FindByIdAsync(id);
        public Task<User> FindByEmail(string email) => _userManager.FindByEmailAsync(email);
        public Task<User> FindByName(string userName) => _userManager.FindByNameAsync(userName);
        public Task<IdentityResult> Create(User user, string password) => _userManager.CreateAsync(user, password);
        public Task<IdentityResult> Update(User user) => _userManager.UpdateAsync(user);
        public Task<IdentityResult> Delete(User user) => _userManager.DeleteAsync(user);
        public Task<IList<string>> GetRoles(User user) => _userManager.GetRolesAsync(user);
        public Task<IdentityResult> AddToRole(User user, string role) => _userManager.AddToRoleAsync(user, role);
        public Task<IdentityResult> RemoveFromRoles(User user, IEnumerable<string> roles) => _userManager.RemoveFromRolesAsync(user, roles);
        public Task<SignInResult> PasswordSignIn(string userName, string password, bool isPersistent, bool lockoutOnFailure) =>
            _signInManager.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
        public Task SignIn(User user, bool isPersistent) => _signInManager.SignInAsync(user, isPersistent);
        public Task SignOut() => _signInManager.SignOutAsync();
    }
}
