/*
 *      @Author: yaile
 */

using ITrack.Service.Interfaces;

namespace ITrack.Data
{
    public static class DataSeeder
    {
        public static async Task EnsureSeed(IServiceProvider provider)
        {
            var dataSeederService = provider.GetRequiredService<IDataSeederService>();

            await dataSeederService.SeedRolesAsync();
            await dataSeederService.SeedAdminUserAsync();
        }
    }
}
