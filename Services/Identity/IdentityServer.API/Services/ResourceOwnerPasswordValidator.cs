using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer.API.Models;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace IdentityServer.API.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        // repository to get user from db
        private readonly IUserDataModel userDataModel;

        public ResourceOwnerPasswordValidator(IUserDataModel userDataModel)
        {
            this.userDataModel = userDataModel;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var user = await userDataModel.GetUser(context?.UserName).ConfigureAwait(true);
                if (user != null)
                {
                    if (user.Password.ToSha256() == context.Password)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(JwtClaimTypes.PreferredUserName, user.Username ?? string.Empty),
                        };

                        //set the result
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
                Console.WriteLine($"{GetType().Name}:Erorr {ex}");
                throw;
            }
        }
    }
}
