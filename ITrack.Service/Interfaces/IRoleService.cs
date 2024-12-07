/*
 *      @Author: yaile
 */

using Microsoft.AspNetCore.Identity;

namespace ITrack.Service.Interfaces;

public interface IRoleService
{
    IQueryable<IdentityRole> GetAll();
    Task<IdentityRole> GetRoleByName(string roleName);
    Task<IdentityResult> CreateRole(IdentityRole role);
    Task<bool> RoleExists(string roleName);
}