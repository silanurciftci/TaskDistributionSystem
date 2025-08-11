using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskDistributionSystem.Models;

namespace TaskDistributionSystem.Controllers
{
    public class PersonelController : Controller
    {
        private readonly AppDbContext _context;
        public PersonelController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var list = await _context.Personeller.AsNoTracking()
                          .OrderBy(p => p.Ad).ThenBy(p => p.Soyad)
                          .ToListAsync();
            ViewData["Title"] = "Personel Listesi";
            return View(list);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();
            var p = await _context.Personeller.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == id.Value);
            if (p == null) return NotFound();
            ViewData["Title"] = "Personel Detay";
            return View(p);
        }

        public IActionResult Create()
        {
            ViewData["Title"] = "Yeni Personel";
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ad,Soyad")] Personel personel)
        {
            if (!ModelState.IsValid) return View(personel);
            _context.Personeller.Add(personel);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Personel oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return NotFound();
            var p = await _context.Personeller.FindAsync(id.Value);
            if (p == null) return NotFound();
            ViewData["Title"] = "Personel Düzenle";
            return View(p);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ad,Soyad")] Personel personel)
        {
            if (id != personel.Id) return NotFound();
            if (!ModelState.IsValid) return View(personel);

            _context.Entry(personel).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Personel güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();
            var p = await _context.Personeller.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == id.Value);
            if (p == null) return NotFound();
            ViewData["Title"] = "Personel Sil";
            return View(p);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var gorevler = await _context.Gorevler.Where(g => g.PersonelId == id).ToListAsync();
            if (gorevler.Count > 0)
            {
                foreach (var g in gorevler) g.PersonelId = null;
                await _context.SaveChangesAsync();
            }

            var p = await _context.Personeller.FindAsync(id);
            if (p != null)
            {
                _context.Personeller.Remove(p);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Personel silindi.";
            }
            else TempData["Error"] = "Personel bulunamadı.";
            return RedirectToAction(nameof(Index));
        }
    }
}
