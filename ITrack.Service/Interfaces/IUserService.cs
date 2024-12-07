/*
 *      @Author: yaile
 */

using ITrack.Data.Entities;
using ITrack.Data.Responce;
using ITrack.Data.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace ITrack.Service.Interfaces;

public interface IUserService
{
    IQueryable<User> GetAll();
    Task<User> GetUserById(string id);
    Task<User> GetUserByEmail(string email);
    Task<User> GetUserByName(string userName);
    Task<IdentityResult> CreateUser(User user, string password);
    Task<IBaseResponse<RegisterViewModel>> CreateUser(RegisterViewModel model);
    Task<IBaseResponse<EditUserViewModel>> UpdateUser(EditUserViewModel model);
    Task<IBaseResponse<bool>> DeleteUser(string id);
    Task<IList<string>> GetUserRoles(User user);
    Task<IdentityResult> AddUserToRole(User user, string role);
    Task<IdentityResult> RemoveUserFromRoles(User user, IEnumerable<string> roles);
    Task<SignInResult> PasswordSignIn(string userName, string password, bool isPersistent, bool lockoutOnFailure);
    Task SignInUser(User user, bool isPersistent);
    Task SignOutUser();
}