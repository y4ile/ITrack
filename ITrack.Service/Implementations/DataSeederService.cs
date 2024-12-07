/*
 *      @Author: yaile
 */

using ITrack.Data.Entities;
using ITrack.Data.Roles;
using ITrack.Service.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ITrack.Service.Implementations;

public class DataSeederService : IDataSeederService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;

    public DataSeederService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }
    
    public async Task SeedRolesAsync()
    {
        // [INFO] Creating default roles
        foreach (var role in RoleNames.AllRoles)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(role));
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
        }
    }

    public async Task SeedAdminUserAsync()
    {
        // [INFO] Creating admin user
        var adminEmail = "playzet.srsly@mail.ru";
        var adminUser = await _userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var user = new User
            {
                Email = adminEmail,
                EmailConfirmed = true,
                UserName = "yaile"
            };

            var result = await _userManager.CreateAsync(user, "12327sub*");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, RoleNames.Admin);
            }
            else throw new Exception(result.Errors.First().Description);
        }
    }
}