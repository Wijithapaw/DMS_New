using DMS.Domain.Dtos.Account;
using DMS.Domain.Dtos.User;
using DMS.Domain.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMS.WebApi.Controllers
{

    [Authorize]
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        IIdentityService _accountService;

        public AccountController(IIdentityService accountService)
        {
            _accountService = accountService;
        }

        [Authorize("ManageAccounts")]
        [HttpPost("Register")]
        public async Task<int> RegisterUser(UserDto userDto)
        {
            var id = await _accountService.RegisterUserAsync(userDto);
            return id;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<AuthResultDto> Login([FromBody] LoginCredentialsDto loginDto)
        {
            var authToken = await _accountService.AuthenticateAsync(loginDto);
            return authToken;
        }
       
        [HttpGet("CurrentUser")]
        public async Task<UserLDto> GetCurrentUser()
        {
            var user = await _accountService.GetCurentUser(this.User);
            return user;
        }
    }
}