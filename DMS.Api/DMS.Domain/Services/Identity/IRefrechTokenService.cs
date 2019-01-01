using System.Threading.Tasks;

namespace DMS.Domain.Services.Identity
{
    public interface IRefreshTokenService
    {
        Task<string> CreateAsync(int userId, string authToken);

        Task<string> GetAsync(int userId, string authToken);
    }
}
