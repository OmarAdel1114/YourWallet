using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace YourWallet.Controllers;

[Authorize]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        var name = User.FindFirstValue(ClaimTypes.Name);
        var email = User.FindFirstValue(ClaimTypes.Email);
        var picture = User.FindFirstValue("picture");

        ViewData["UserName"] = name;
        ViewData["UserEmail"] = email;
        ViewData["UserPicture"] = picture;

        return View();
    }
}