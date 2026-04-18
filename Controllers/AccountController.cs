// Controllers/AccountController.cs
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YourWallet.Services.Interfaces;

namespace YourWallet.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    // GET /Account/Login
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    // POST /Account/LoginWithGoogle
    // This triggers the Google OAuth redirect
    public IActionResult LoginWithGoogle(string? returnUrl = null)
    {
        var redirectUrl = Url.Action(nameof(GoogleCallback), "Account", new { returnUrl });
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    // GET /Account/GoogleCallback  ← Google redirects here after login
    public async Task<IActionResult> GoogleCallback(string? returnUrl = null)
    {
        // Read the result from the external (Google) cookie
        var result = await HttpContext.AuthenticateAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        if (!result.Succeeded)
            return RedirectToAction(nameof(Login));

        // Find or create the user in our database
        var appUser = await _userService.FindOrCreateUserAsync(result.Principal!);

        // Build OUR app's claims (what we store in our session cookie)
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
            new Claim(ClaimTypes.Email, appUser.Email),
            new Claim(ClaimTypes.Name, appUser.DisplayName),
        };

        if (appUser.ProfilePictureUrl is not null)
            claims.Add(new Claim("picture", appUser.ProfilePictureUrl));

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        // Sign in — this creates the session cookie
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties { IsPersistent = true });

        // Redirect safely (prevent open redirect attacks)
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return LocalRedirect(returnUrl);

        return RedirectToAction("Index", "Expenses");
    }

    // POST /Account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }
}