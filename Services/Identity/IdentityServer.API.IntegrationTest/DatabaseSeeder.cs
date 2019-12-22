using System;
using System.Threading.Tasks;
using IdentityServer.API.Domains;
using IdentityServer.API.Models;

namespace IdentityServer.API.IntegrationTest
{
    public class DatabaseSeeder
    {
        private readonly IUserDataModel _userManager;

        public DatabaseSeeder(IUserDataModel userManager)
        {
            _userManager = userManager;
        }

        public async Task Seed()
        {
            // Add all the predefined profiles using the predefined password
            foreach (var profile in PredefinedData.Profiles)
            {
                await _userManager.InsertUser(profile);
            }
        }
    }
}
