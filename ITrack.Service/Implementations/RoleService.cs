/*
 *      @Author: yaile
 */

using ITrack.Data.Interfaces;
using ITrack.Service.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ITrack.Service.Implementations;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public IQueryable<IdentityRole> GetAll() => _roleRepository.GetAll();
    public Task<IdentityRole> GetRoleByName(string roleName) => _roleRepository.FindByName(roleName);
    public Task<IdentityResult> CreateRole(IdentityRole role) => _roleRepository.Create(role);
    public Task<bool> RoleExists(string roleName) => _roleRepository.RoleExists(roleName);
}