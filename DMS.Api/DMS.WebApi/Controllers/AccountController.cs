using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DMS.Domain.Dtos;
using DMS.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using DMS.Domain.Dtos.Account;
using DMS.Domain.Dtos.User;

namespace DMS.WebApi.Controllers
{
    
    [Authorize]
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize("ManageAccounts")]
        [HttpPost("Register")]
        public async Task<int> RegisterUser(UserDto userDto)
        {
            var id = await _accountService.RegisterUser(userDto);
            return id;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<AuthToken> Login([FromBody] LoginDto loginDto)
        {
            var authToken = await _accountService.CreateToken(loginDto);
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