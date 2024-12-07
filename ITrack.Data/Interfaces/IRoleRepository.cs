/*
 *      @Author: yaile
 */

using Microsoft.AspNetCore.Identity;

namespace ITrack.Data.Interfaces;

public interface IRoleRepository
{
    IQueryable<IdentityRole> GetAll();
    Task<IdentityRole> FindByName(string roleName);
    Task<IdentityResult> Create(IdentityRole role);
    Task<bool> RoleExists(string roleName);
}