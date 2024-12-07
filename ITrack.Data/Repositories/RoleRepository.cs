/*
 *      @Author: yaile
 */

using ITrack.Data.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ITrack.Data.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleRepository(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }
    
    public IQueryable<IdentityRole> GetAll() => _roleManager.Roles;
    public Task<IdentityRole> FindByName(string roleName) => _roleManager.FindByNameAsync(roleName);
    public Task<IdentityResult> Create(IdentityRole role) => _roleManager.CreateAsync(role);
    public Task<bool> RoleExists(string roleName) => _roleManager.RoleExistsAsync(roleName);
}