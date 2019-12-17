using System;
using System.Net;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer.API.Domains;
using IdentityServer.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace IdentityServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserDataModel userDataModel;
        private readonly ILogger logger;

        public AccountController(IUserDataModel userDataModel)
        {
            this.userDataModel = userDataModel;
        }

        public AccountController(IUserDataModel userDataModel, ILogger logger)
        {
            this.userDataModel = userDataModel;
            this.logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> RegisterUser(User user)
        {
            try
            {
                if (user == null)
                {
                    logger?.Debug($"Response: <{nameof(BadRequest)}>, given user is null");
                    return BadRequest("User not specified");
                }

                var userExist = await userDataModel.GetUser(user.Username).ConfigureAwait(true);
                if (userExist == null)
                {
                    user.Password = user.Password.ToSha256();
                    await userDataModel.InsertUser(user).ConfigureAwait(true);

                    logger.Debug($"Response <{nameof(Ok)}>, User with username <{user.Username}> created");
                    return Ok("User created");
                }

                logger.Debug($"Response <{nameof(Conflict)}>>, User with username <{user.Username}> already exists");
                return Conflict("User already exist");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "{0}", GetType().Name);
                throw;
            }
        }

        [HttpDelete]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> DeleteUser(User user)
        {
            try
            {
                if (user == null)
                {
                    logger.Debug($"Response: <{nameof(BadRequest)}>, given user is null");
                    return BadRequest("User not specified");
                }

                var userExist = await userDataModel.GetUser(user.Username).ConfigureAwait(true);
                if (userExist != null)
                {
                    await userDataModel.DeleteUser(user.Username).ConfigureAwait(true);

                    logger.Debug($"Response: <{nameof(Ok)}>, user with username <{user.Username}> was deleted");
                    return Ok("User deleted");
                }

                logger.Debug($"Response <{nameof(NotFound)}>>, User with username <{user.Username}> not exists");
                return NotFound("User not exist");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "{0}", GetType().Name);
                throw;
            }
        }
    }
}
