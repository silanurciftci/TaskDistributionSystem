using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskDistributionSystem.Models;

namespace TaskDistributionSystem.Controllers
{
    public class IslemController : Controller
    {
        private readonly AppDbContext _context;
        public IslemController(AppDbContext context) => _context = context;

        
        public async Task<IActionResult> Index()
        {
            var list = await _context.Islemler
                                     .AsNoTracking()
                                     .OrderBy(x => x.Zorluk)
                                     .ThenBy(x => x.Ad)
                                     .ToListAsync();
            ViewData["Title"] = "İşlemler";
            return View(list);
        }

      
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var islem = await _context.Islemler.AsNoTracking()
                                .FirstOrDefaultAsync(m => m.Id == id.Value);
            if (islem == null) return NotFound();

            ViewData["Title"] = "İşlem Detay";
            return View(islem);
        }

        
        public IActionResult Create()
        {
            ViewData["Title"] = "Yeni İşlem";
            return View();
        }

        
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ad,Zorluk")] Islem islem)
        {
            if (!ModelState.IsValid) return View(islem);

            _context.Islemler.Add(islem);
            await _context.SaveChangesAsync();

            TempData["Success"] = "İşlem oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return NotFound();
            var islem = await _context.Islemler.FindAsync(id.Value);
            if (islem == null) return NotFound();

            ViewData["Title"] = "İşlem Düzenle";
            return View(islem);
        }

        
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ad,Zorluk")] Islem islem)
        {
            if (id != islem.Id) return NotFound();
            if (!ModelState.IsValid) return View(islem);

            try
            {
                _context.Entry(islem).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                TempData["Success"] = "İşlem güncellendi.";
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await _context.Islemler.AnyAsync(e => e.Id == id);
                if (!exists) return NotFound();
                TempData["Error"] = "Güncelleme sırasında eşzamanlılık hatası oluştu.";
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var islem = await _context.Islemler.AsNoTracking()
                                .FirstOrDefaultAsync(m => m.Id == id.Value);
            if (islem == null) return NotFound();

            ViewData["Title"] = "İşlem Sil";
            return View(islem);
        }

        
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var bagliGorevSayisi = await _context.Gorevler.CountAsync(g => g.IslemId == id);
            if (bagliGorevSayisi > 0)
            {
                TempData["Error"] = $"Bu işleme bağlı {bagliGorevSayisi} görev var. Önce görevleri değiştirin ya da silin.";
                return RedirectToAction(nameof(Index));
            }

            var islem = await _context.Islemler.FindAsync(id);
            if (islem != null)
            {
                _context.Islemler.Remove(islem);
                await _context.SaveChangesAsync();
                TempData["Success"] = "İşlem silindi.";
            }
            else
            {
                TempData["Error"] = "İşlem zaten bulunamadı.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
