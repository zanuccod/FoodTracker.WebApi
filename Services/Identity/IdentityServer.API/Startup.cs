using IdentityServer.API.Configuration;
using IdentityServer.API.Models;
using IdentityServer.API.Services;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            ConfigureDatabaseServices(services);

            services.AddSingleton<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddSingleton<IProfileService, ProfileService>();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(new IdentityResource[]
                    {
                        new IdentityResources.OpenId()
                    })
                .AddInMemoryApiResources(ApiResources.GetApiResources())
                .AddInMemoryClients(Clients.Get())
                .AddJwtBearerClientAuthentication()
                .AddProfileService<ProfileService>();

            // local api endpoint
            services.AddLocalApiAuthentication();
        }

        public virtual void ConfigureDatabaseServices(IServiceCollection services)
        {
            services.AddSingleton<IUserDataModel, EFUserDataModel>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
