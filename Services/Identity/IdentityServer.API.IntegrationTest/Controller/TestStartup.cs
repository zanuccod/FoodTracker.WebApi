using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using IdentityServer.API.Models;
using IdentityServer.API.Controllers;

namespace IdentityServer.API.IntegrationTest.Controller
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration)
            :base(configuration)
        { }

        public override void ConfigureDatabaseServices(IServiceCollection services)
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            services.AddSingleton<IUserDataModel>(new EFUserDataModel(options));

            services.AddTransient<DatabaseSeeder>();

            // add this to prevent "404-NotFound" error when controller are on separate assembly
            // add an entry for every controller
            services.AddControllers().AddApplicationPart(typeof(AccountController).Assembly);
        }

        public override void Configure(IApplicationBuilder app)
        {
            base.Configure(app);

            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var seeder = serviceScope.ServiceProvider.GetService<DatabaseSeeder>();
            seeder.Seed().ConfigureAwait(true);
        }
    }
}
