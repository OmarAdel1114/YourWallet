using System.Security.Claims;
using YourWallet.Models.Domain;

namespace YourWallet.Services.Interfaces;

public interface IUserService
{
    Task<AppUser> FindOrCreateUserAsync(ClaimsPrincipal principal);
}