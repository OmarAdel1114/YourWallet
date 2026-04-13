using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourWallet.Data;
using YourWallet.Data.Service;
using YourWallet.Models;

namespace YourWallet.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly IExpensesService _expesesService;
        public ExpensesController(IExpensesService expesesService)
        {
            _expesesService = expesesService;
        }
        public async Task<IActionResult> Index()
        {
            var expenses = await _expesesService.GetAll();
            return View(expenses);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Expense expense)
        {
            if(ModelState.IsValid)
            {
               await _expesesService.Add(expense);

                return RedirectToAction("Index");
            }
            return View(expense);
        }
        public IActionResult GetChart()
        {
            var data = _expesesService.GetCharData();
            return Json(data);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _expesesService.DeleteExpenseAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
