using Microsoft.AspNetCore.Mvc;
using SpendSmart.Data;
using SpendSmart.Models;
using System.Diagnostics;

namespace SpendSmart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SpendSmartDbContext _context;

        public HomeController(ILogger<HomeController> logger, SpendSmartDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Expenses()
        {
            var expenses = _context.Expenses.ToList();
            var totalExpenses = expenses.Sum(e => e.Value);
            ViewBag.EXPENSES = totalExpenses;
            return View(expenses);
        }
        public IActionResult CreateOrEditExpense(int? id)
        {
            if (id != null)
            {
                var expense = _context.Expenses.FirstOrDefault(e => e.Id == id);
                return View(expense);
            }
            return View();
        }

        public IActionResult DeleteExpense(int id)
        {
            var expense = _context.Expenses.FirstOrDefault(e => e.Id == id);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                _context.SaveChanges();
            }

            return RedirectToAction("Expenses");
        }

        public IActionResult CreateOrEditExpenseForm(Expense model)
        {
            if (model.Id == 0)
            {
                _context.Expenses.Add(model);
            }
            else
            {
                _context.Expenses.Update(model);
            }
            _context.SaveChangesAsync();
            return RedirectToAction("Expenses");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
