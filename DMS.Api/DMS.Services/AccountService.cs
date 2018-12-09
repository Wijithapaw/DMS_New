using DMS.Data.Entities;
using DMS.Domain.Dtos.Account;
using DMS.Domain.Dtos.User;
using DMS.Domain.Services;
using DMS.Utills;
using DMS.Utills.ConfigSettings;
using DMS.Utills.CustomExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEnvironmentDescriptor _env;
        private readonly JwtSettings _jwtSettings;
        private readonly AppSettings _appSettings;

        public AccountService(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signinManager,
            RoleManager<ApplicationRole> roleManager,
            IHttpContextAccessor httpContextAccessor,
            IEnvironmentDescriptor env,
            IOptions<JwtSettings> jwtSettings,
            IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _env = env;
            _jwtSettings = jwtSettings.Value;
            _appSettings = appSettings.Value;
        }

        public async Task<AuthToken> CreateToken(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user != null)
            {
                var signinResult = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

                if (signinResult.Succeeded)
                {
                    var userClaims = await GetUserClaims(user);

                    var userRoles = await _userManager.GetRolesAsync(user);
                    var rolesClaims = userRoles.Select(r => new Claim(ClaimTypes.Role, r)).ToArray();

                    var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Name, user.UserName)
                    }.Union(userClaims).Union(rolesClaims);

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SigningKey));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: _jwtSettings.Issuer,
                        audience: _jwtSettings.Audience,
                        claims: claims,
                        expires: DateTime.UtcNow.AddMinutes(15),
                        signingCredentials: creds
                        );

                    var authToken = new AuthToken
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Expiration = token.ValidTo
                    };

                    return authToken;
                }
            }

            throw new UnsuccessfulLoginException();
        }

        public async Task<UserLDto> GetCurentUser(ClaimsPrincipal userClaims)
        {
            var user = await _userManager.GetUserAsync(userClaims);

            var roles =  await _userManager.GetRolesAsync(user);

            var permissionsClaims = await GetUserClaims(user);

            var userDto = new UserLDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Active = user.Active,
                Birthday = user.Birthday,
                Email = user.Email,
                Roles = roles.ToArray(),
                PermissionClaims = permissionsClaims.Select(c => c.Value).ToArray()
            };

            return userDto;
        }

        public async Task<int> RegisterUser(UserDto userDto)
        {
            var user = new ApplicationUser
            {
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Birthday = userDto.Birthday,
                Active = userDto.Active,
                UserName = userDto.Email,
                CreatedBy = _env.UserId,
                CreatedDate = DateTime.Now,
                LastUpdatedBy = _env.UserId,
                LastUpdatedDate = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, _appSettings.DefaultPassword);
            if (result.Succeeded)
                return user.Id;

            throw new DMSException("Error creating user");
        }

        #region Private Methods

        private async Task<IEnumerable<Claim>> GetUserClaims(ApplicationUser user)
        {
            var userClaims = (IEnumerable<Claim>)(await _userManager.GetClaimsAsync(user));

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var roleName in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                userClaims = userClaims.Union(roleClaims);
            }

            return userClaims;
        }

        #endregion  
    }
}
