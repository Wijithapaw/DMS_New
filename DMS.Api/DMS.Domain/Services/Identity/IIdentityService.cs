using DMS.Domain.Dtos.Account;
using DMS.Domain.Dtos.User;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Services.Identity
{
    public interface IIdentityService
    {
        Task<int> RegisterUserAsync(UserDto userDto);

        Task<AuthResultDto> AuthenticateAsync(LoginCredentialsDto loginDto);

        Task<UserLDto> GetCurentUser(ClaimsPrincipal user);
    }
}
