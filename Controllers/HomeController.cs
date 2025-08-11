using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskDistributionSystem.Models;

namespace TaskDistributionSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            ViewBag.PersonelCount = await _db.Personeller.CountAsync();
            ViewBag.IslemCount    = await _db.Islemler.CountAsync();
            ViewBag.GorevCount    = await _db.Gorevler.CountAsync();
            ViewData["Title"]     = "Özet Panel";
            return View();
        }
    }
}
