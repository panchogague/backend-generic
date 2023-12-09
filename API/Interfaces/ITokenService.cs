using Microsoft.AspNetCore.Identity;

namespace Chanchas.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(IdentityUser user);
    }
}