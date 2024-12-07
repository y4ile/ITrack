/*
 *      @Author: yaile
 */

using ITrack.Data.Entities;
using ITrack.Data.Enum;
using ITrack.Data.Interfaces;
using ITrack.Data.Responce;
using ITrack.Data.Roles;
using ITrack.Data.ViewModels;
using ITrack.Service.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ITrack.Service.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public IQueryable<User> GetAll() => _userRepository.GetAll();
    public Task<User> GetUserById(string id) => _userRepository.FindById(id);
    public Task<User> GetUserByEmail(string email) => _userRepository.FindByEmail(email);
    public Task<User> GetUserByName(string userName) => _userRepository.FindByName(userName);
    public Task<IdentityResult> CreateUser(User user, string password) => _userRepository.Create(user, password);
    
    public async Task<IBaseResponse<RegisterViewModel>> CreateUser(RegisterViewModel model)
    {
        var baseResponse = new BaseResponse<RegisterViewModel>();
        try
        {
            var user = new User { UserName = model.Username, Email = model.Email };
            var result = await _userRepository.Create(user, model.Password);
            if (result.Succeeded)
            {
                // [INFO] Signing in user
                await _userRepository.AddToRole(user, RoleNames.User);
                await _userRepository.SignIn(user, isPersistent: false);
                baseResponse.StatusCode = StatusCode.OK;
                return baseResponse;
            }
            
            baseResponse.Description = "Identity result failed";
            baseResponse.StatusCode = StatusCode.IdentityResultFailed;
                                    
            return baseResponse;
        }
        catch (Exception e)
        {
            return new BaseResponse<RegisterViewModel>()
            {
                Description = $"[Edit] : {e.Message}",
                StatusCode = StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<EditUserViewModel>> UpdateUser(EditUserViewModel model)
    {
        var baseResponse = new BaseResponse<EditUserViewModel>();
        try
        {
            var user = await _userRepository.FindById(model.Id);

            if (user == null)
            {
                baseResponse.Description = "User not found";
                baseResponse.StatusCode = StatusCode.UserNotFound;
                                    
                return baseResponse;
            }
            
            user.Email = model.Email;
            user.UserName = model.UserName;
                                    
            var userRoles = await _userRepository.GetRoles(user);
            var removeResult = await _userRepository.RemoveFromRoles(user, userRoles);
            if (!removeResult.Succeeded)
            {
                baseResponse.Description = "Identity result failed";
                baseResponse.StatusCode = StatusCode.IdentityResultFailed;
                                    
                return baseResponse;
            }
                                    
            var addResult = await _userRepository.AddToRole(user, model.SelectedRole);
            if (!addResult.Succeeded)
            {
                baseResponse.Description = "Identity result failed";
                baseResponse.StatusCode = StatusCode.IdentityResultFailed;
                                    
                return baseResponse;
            }
                                    
            var updateResult = await _userRepository.Update(user);
            if (!updateResult.Succeeded)
            {
                baseResponse.Description = "Identity result failed";
                baseResponse.StatusCode = StatusCode.IdentityResultFailed;
                                    
                return baseResponse;
            }
            
            baseResponse.StatusCode = StatusCode.OK;
            return baseResponse;
        }
        catch (Exception e)
        {
            return new BaseResponse<EditUserViewModel>()
            {
                Description = $"[Edit] : {e.Message}",
                StatusCode = StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<bool>> DeleteUser(string id)
    {
        var baseResponse = new BaseResponse<bool>()
        {
            Data = true
        };
        try
        {
            var user = await _userRepository.FindById(id);
            if (user == null)
            {
                baseResponse.Description = "User not found";
                baseResponse.StatusCode = StatusCode.UserNotFound;
                return baseResponse;
            }
            
            var userRoles = await _userRepository.GetRoles(user);
            if (!userRoles.Contains(RoleNames.Admin))
            {
                var deleteResult = await _userRepository.Delete(user);
                if (!deleteResult.Succeeded)
                {
                    baseResponse.Description = "Identity result failed";
                    baseResponse.StatusCode = StatusCode.IdentityResultFailed;
                                    
                    return baseResponse;
                }
            }
            
            baseResponse.StatusCode = StatusCode.OK;
            return baseResponse;
        }
        catch (Exception e)
        {
            return new BaseResponse<bool>()
            {
                Description = $"[Delete] : {e.Message}",
                StatusCode = StatusCode.InternalServerError
            };
        }
    }
    public Task<IList<string>> GetUserRoles(User user) => _userRepository.GetRoles(user);
    public Task<IdentityResult> AddUserToRole(User user, string role) => _userRepository.AddToRole(user, role);
    public Task<IdentityResult> RemoveUserFromRoles(User user, IEnumerable<string> roles) => _userRepository.RemoveFromRoles(user, roles);
    public Task<SignInResult> PasswordSignIn(string userName, string password, bool isPersistent, bool lockoutOnFailure) =>
        _userRepository.PasswordSignIn(userName, password, isPersistent, lockoutOnFailure);
    public Task SignInUser(User user, bool isPersistent) => _userRepository.SignIn(user, isPersistent);
    public Task SignOutUser() => _userRepository.SignOut();
}