using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer.API.Models;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.Extensions.Logging;

namespace IdentityServer.API.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserDataModel userDataModel;
        private readonly ILogger<ResourceOwnerPasswordValidator> logger;

        public ResourceOwnerPasswordValidator(IUserDataModel userDataModel, ILogger<ResourceOwnerPasswordValidator> logger)
        {
            this.userDataModel = userDataModel;
            this.logger = logger;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }

                var user = await userDataModel.GetUser(context.UserName).ConfigureAwait(true);

                if (user != null)
                {
                    if (user.Password == context.Password)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(JwtClaimTypes.PreferredUserName, user.Username ?? string.Empty),
                        };

                        // set the result
                        context.Result = new GrantValidationResult(
                            subject: user.Username,
                            authenticationMethod: "custom",
                            claims: claims);

                        return;
                    }

                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Incorrect password");
                    return;
                }
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "User does not exist.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, GetType().Name);
                throw;
            }
        }
    }
}
