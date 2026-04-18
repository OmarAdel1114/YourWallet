using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YourWallet.Models;
using YourWallet.Services;

namespace YourWallet.Controllers
{
    [Authorize] 
    public class ExpensesController : Controller
    {
        private readonly IExpensesService _expesesService;

        public ExpensesController(IExpensesService expesesService)
        {
            _expesesService = expesesService;
        }

        // Helper — reads the logged-in user's ID from their cookie claims
        private int GetCurrentUserId()
        {
            var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(value!);
        }

        public async Task<IActionResult> Index()
        {
            var expenses = await _expesesService.GetAll(GetCurrentUserId());
            return View(expenses);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Expense expense)
        {
            ModelState.Remove("AppUser");
            ModelState.Remove("AppUserId");

            if (ModelState.IsValid)
            {
                await _expesesService.Add(expense, GetCurrentUserId());
                return RedirectToAction("Index");
            }
            return View(expense);
        }

        public IActionResult GetChart()
        {
            var data = _expesesService.GetCharData(GetCurrentUserId());
            return Json(data);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _expesesService.DeleteExpenseAsync(id, GetCurrentUserId());
            return RedirectToAction(nameof(Index));
        }
    }
}