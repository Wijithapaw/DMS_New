using DMS.Domain;
using DMS.Domain.Entities.Identity;
using DMS.Domain.Services;
using DMS.Domain.Services.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Services.Identity
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IDataContext _dataContext;

        public RefreshTokenService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<string> CreateAsync(int userId, string authToken)
        {
            var currentRefreshToken = await _dataContext.RefreshTokens
                .Where(t => t.UserId == userId && t.AuthToken == authToken)
                .FirstOrDefaultAsync();

            if (currentRefreshToken != null)
            {
                _dataContext.RefreshTokens.Remove(currentRefreshToken);
            }

            var refreshToken = new RefreshToken
            {
                AuthToken = authToken,
                Token = GenerateRefreshToken(),
                UserId = userId
            };
            _dataContext.RefreshTokens.Add(refreshToken);

            await _dataContext.SaveChangesAsync();

            return refreshToken.Token;
        }

        public async Task<string> GetAsync(int userId, string authToken)
        {
            var refreshToken = await _dataContext.RefreshTokens
                .Where(t => t.UserId == userId && t.AuthToken == authToken)
                .FirstOrDefaultAsync();

            var token = refreshToken?.Token;

            return token;
        }

        #region Private Methods

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        #endregion
    }
}
