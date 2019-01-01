using DMS.Domain;
using DMS.Domain.ConfigSettings;
using DMS.Domain.CustomExceptions;
using DMS.Domain.Dtos.Account;
using DMS.Domain.Dtos.User;
using DMS.Domain.Entities.Identity;
using DMS.Domain.Services.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signinManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IRequestContext _requestContext;
        private readonly JwtSettings _jwtSettings;
        private readonly AppSettings _appSettings;

        public IdentityService(UserManager<User> userManager, 
            SignInManager<User> signinManager,
            RoleManager<Role> roleManager,
            IRefreshTokenService  refreshTokenService,
            IRequestContext requestContext,
            IOptions<JwtSettings> jwtSettings,
            IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _roleManager = roleManager;
            _refreshTokenService = refreshTokenService;

            _requestContext = requestContext;
            _jwtSettings = jwtSettings.Value;
            _appSettings = appSettings.Value;
        }

        public async Task<AuthResultDto> AuthenticateAsync(LoginCredentialsDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user != null)
            {
                var signinResult = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

                if (signinResult.Succeeded)
                {
                    var claims = await GetAllClaimsOfUserAsync(user);

                    var token = CreateJwtToken(claims);

                    var refreshToken = await _refreshTokenService.CreateAsync(user.Id, token.token);

                    var authToken = new AuthResultDto
                    {
                        Succeeded = true,
                        AuthToken = token.token,
                        RefreshToken = refreshToken,
                        Expiration = token.expires
                    };

                    return authToken;
                }
            }

            return new AuthResultDto
            {
                Succeeded = false,
                ErrorCode = "ERR_INVALID_LOGIN_ATTEMPT"
            };
        }

        public async Task<AuthResultDto> RefreshTokenAsync(RefreshTokenDto dto)
        {
            try
            {
                var claimsPrinciple = GetPrincipalFromExpiredToken(dto.AuthToken);

                var userId = int.Parse(claimsPrinciple.FindFirstValue(ClaimTypes.NameIdentifier));

                var currentRefreshToken = await _refreshTokenService.GetAsync(userId, dto.AuthToken);

                if (currentRefreshToken != dto.RefreshToken)
                    throw new SecurityTokenException();

                var newAuthToken = CreateJwtToken(claimsPrinciple.Claims);

                var newRefreshToken = await _refreshTokenService.CreateAsync(userId, newAuthToken.token);

                return new AuthResultDto
                {
                    Succeeded = true,
                    AuthToken = newAuthToken.token,
                    Expiration = newAuthToken.expires,
                    RefreshToken = newRefreshToken
                };
            }
            catch (SecurityTokenException)
            {
                return new AuthResultDto
                {
                    Succeeded = false
                };
            }
        }

        public async Task<int> RegisterUserAsync(UserDto userDto)
        {
            var user = new User
            {
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Birthday = userDto.Birthday,
                Active = userDto.Active,
                UserName = userDto.Email,
                CreatedBy = _requestContext.UserId,
                CreatedDateUtc = DateTime.Now,
                LastUpdatedBy = _requestContext.UserId,
                LastUpdatedDateUtc = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, _appSettings.DefaultPassword);
            if (result.Succeeded)
                return user.Id;

            throw new DMSException("Error creating user");
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

        #region Private Methods

        private async Task<IEnumerable<Claim>> GetUserClaims(User user)
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

        private async Task<IEnumerable<Claim>> GetAllClaimsOfUserAsync(User user)
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

            return claims;
        }

        private (string token, DateTime expires) CreateJwtToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(15);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return (tokenStr, expires);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidAudience = _jwtSettings.Audience,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SigningKey)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        #endregion  
    }
}
