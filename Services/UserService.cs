using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using YourWallet.Data;
using YourWallet.Models.Domain;
using YourWallet.Services.Interfaces;

namespace YourWallet.Services;

public class UserService : IUserService
{
    private readonly YourWalletContext _db;

    public UserService(YourWalletContext db)
    {
        _db = db;
    }

    public async Task<AppUser> FindOrCreateUserAsync(ClaimsPrincipal principal)
    {
        // Extract claims that Google sends back
        var googleId = principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("Google ID claim missing.");

        var email = principal.FindFirstValue(ClaimTypes.Email)
            ?? throw new InvalidOperationException("Email claim missing.");

        var displayName = principal.FindFirstValue(ClaimTypes.Name) ?? email;
        var picture = principal.FindFirstValue("picture");

        // Check if user already exists
        var existingUser = await _db.Users
            .FirstOrDefaultAsync(u => u.GoogleId == googleId);

        if (existingUser is not null)
            return existingUser;

        // First time login — create the user
        var newUser = new AppUser
        {
            GoogleId = googleId,
            Email = email,
            DisplayName = displayName,
            ProfilePictureUrl = picture
        };

        _db.Users.Add(newUser);
        await _db.SaveChangesAsync();

        return newUser;
    }
}