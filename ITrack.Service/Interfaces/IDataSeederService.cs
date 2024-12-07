/*
 *      @Author: yaile
 */

namespace ITrack.Service.Interfaces;

public interface IDataSeederService
{
    Task SeedRolesAsync();
    Task SeedAdminUserAsync();
}