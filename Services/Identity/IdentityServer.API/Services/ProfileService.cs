using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer.API.Models;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using NLog;

namespace IdentityServer.API.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserDataModel userDataModel;
        private readonly ILogger logger;

        public ProfileService(IUserDataModel userDataModel, ILogger logger)
        {
            this.userDataModel = userDataModel;
            this.logger = logger;
        }

        // Get user profile date in terms of claims when calling /connect/userinfo
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }

                // depending on the scope accessing the user data.
                if (!string.IsNullOrEmpty(context.Subject.Identity.Name))
                {
                    //get user from db (in my case this is by email)
                    var user = await userDataModel.GetUser(context.Subject.Identity.Name).ConfigureAwait(true);

                    if (user != null)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(JwtClaimTypes.PreferredUserName, user.Username ?? string.Empty),
                        };

                        // set issued claims to return
                        context.IssuedClaims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
                    }
                }
                else
                {
                    // get subject from context (this was set ResourceOwnerPasswordValidator.ValidateAsync),
                    var username = context.Subject.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.PreferredUserName).Value;

                    if (!string.IsNullOrEmpty(username))
                    {
                        // get user from db (find user by user id)
                        var user = await userDataModel.GetUser(username).ConfigureAwait(true);

                        // issue the claims for the user
                        if (user != null)
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(JwtClaimTypes.PreferredUserName, user.Username ?? string.Empty),
                            };

                            context.IssuedClaims = claims.ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "{0}", GetType().Name);
                throw;
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            try
            {
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }

                // get subject from context (set in ResourceOwnerPasswordValidator.ValidateAsync),
                var userName = context?.Subject.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.PreferredUserName).Value;

                if (!string.IsNullOrEmpty(userName))
                {
                    var user = await userDataModel.GetUser(userName).ConfigureAwait(true);

                    if (user != null)
                    {
                        context.IsActive = true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "{0}", GetType().Name);
                throw;
            }
        }
    }
}
