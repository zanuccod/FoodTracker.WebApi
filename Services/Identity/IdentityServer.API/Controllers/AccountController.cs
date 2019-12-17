using System.Net;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer.API.Domains;
using IdentityServer.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserDataModel userDataModel;

        public AccountController(IUserDataModel userDataModel)
        {
            this.userDataModel = userDataModel;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> RegisterUser(User user)
        {
            if (user == null)
            {
                return BadRequest("User not specified");
            }

            var userExist = await userDataModel.GetUser(user.Username).ConfigureAwait(true);
            if (userExist == null)
            {
                user.Password = user.Password.ToSha256();
                await userDataModel.InsertUser(user).ConfigureAwait(true);

                return Ok("User created");
            }
            return Conflict("User already exist");
        }

        [HttpDelete]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> DeleteUser(User user)
        {
            if (user == null)
            {
                return BadRequest("User not specified");
            }

            var userExist = await userDataModel.GetUser(user.Username).ConfigureAwait(true);
            if (userExist != null)
            {
                await userDataModel.DeleteUser(user.Username).ConfigureAwait(true);
                return Ok("User deleted");
            }
            return NotFound("User not exist");
        }
    }
}
