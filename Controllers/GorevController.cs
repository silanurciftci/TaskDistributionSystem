using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskDistributionSystem.Models;
using TaskDistributionSystem.Services;

namespace TaskDistributionSystem.Controllers
{
    public class GorevController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IAssignmentService _assignment;

        public GorevController(AppDbContext context, IAssignmentService assignment)
        {
            _context = context;
            _assignment = assignment;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _context.Gorevler
                .Include(g => g.Islem)
                .Include(g => g.Personel)
                .AsNoTracking()
                .OrderByDescending(g => g.Tarih)
                .ThenBy(g => g.Ad)
                .ToListAsync();

            ViewData["Title"] = "Görevler";
            return View(list);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var g = await _context.Gorevler
                .Include(x => x.Islem)
                .Include(x => x.Personel)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id.Value);

            if (g == null) return NotFound();

            ViewData["Title"] = "Görev Detay";
            return View(g);
        }

        public async Task<IActionResult> Create()
        {
            await FillDropdowns();
            ViewData["Title"] = "Yeni Görev";
            return View(new Gorev { Tarih = DateTime.Today });
        }

        // Manuel seçim ±1 kuralını ihlal ederse seçim yok sayılır ve sistem atar
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Ad,Tarih,Aciklama,IslemId,PersonelId")] Gorev gorev)
        {
            if (!ModelState.IsValid)
            {
                await FillDropdowns(gorev.PersonelId, gorev.IslemId);
                return View(gorev);
            }

            int hedefZ = await _context.Islemler
                .Where(i => i.Id == gorev.IslemId)
                .Select(i => i.Zorluk)
                .FirstAsync();

            if (gorev.PersonelId.HasValue)
            {
                var lastZ = await _context.Gorevler
                    .Where(g => g.PersonelId == gorev.PersonelId.Value)
                    .OrderByDescending(g => g.Tarih)
                    .ThenByDescending(g => g.Id)
                    .Join(_context.Islemler,
                          g => g.IslemId,
                          i => i.Id,
                          (g, i) => (int?)i.Zorluk)
                    .FirstOrDefaultAsync();

                if (lastZ.HasValue && Math.Abs(lastZ.Value - hedefZ) <= 1)
                {
                    gorev.PersonelId = null;
                    TempData["Error"] =
                        "Ardışık zorluk (±1) kuralı ihlal ediliyor. Sistem adil atama yaptı.";
                }
            }

            _context.Gorevler.Add(gorev);
            await _context.SaveChangesAsync();

            if (gorev.PersonelId is null)
            {
                await _assignment.AssignTaskAsync(gorev.Id);

                var g = await _context.Gorevler
                    .Include(x => x.Personel)
                    .AsNoTracking()
                    .FirstAsync(x => x.Id == gorev.Id);

                TempData["Success"] =
                    $"Görev oluşturuldu ve \"{g.Personel?.Ad} {g.Personel?.Soyad}\" kişisine atandı.";
            }
            else
            {
                TempData["Success"] = "Görev oluşturuldu.";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return NotFound();

            var gorev = await _context.Gorevler.FindAsync(id.Value);
            if (gorev == null) return NotFound();

            await FillDropdowns(gorev.PersonelId, gorev.IslemId);
            ViewData["Title"] = "Görev Düzenle";
            return View(gorev);
        }

        // Manuel değişiklik ±1 kuralını ihlal ederse seçim yok sayılır ve sistem atar
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Ad,Tarih,Aciklama,IslemId,PersonelId")] Gorev gorev)
        {
            if (id != gorev.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                await FillDropdowns(gorev.PersonelId, gorev.IslemId);
                return View(gorev);
            }

            int hedefZ = await _context.Islemler
                .Where(i => i.Id == gorev.IslemId)
                .Select(i => i.Zorluk)
                .FirstAsync();

            if (gorev.PersonelId.HasValue)
            {
                var lastZ = await _context.Gorevler
                    .Where(g => g.PersonelId == gorev.PersonelId.Value)
                    .OrderByDescending(g => g.Tarih)
                    .ThenByDescending(g => g.Id)
                    .Join(_context.Islemler,
                          g => g.IslemId,
                          i => i.Id,
                          (g, i) => (int?)i.Zorluk)
                    .FirstOrDefaultAsync();

                if (lastZ.HasValue && Math.Abs(lastZ.Value - hedefZ) <= 1)
                {
                    gorev.PersonelId = null;
                    TempData["Error"] =
                        "Ardışık zorluk (±1) kuralı ihlal ediliyor. Sistem adil atama yaptı.";
                }
            }

            _context.Entry(gorev).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            if (gorev.PersonelId is null)
            {
                await _assignment.AssignTaskAsync(gorev.Id);
                TempData["Success"] =
                    "Görev güncellendi ve uygun personele atandı.";
            }
            else
            {
                TempData["Success"] = "Görev güncellendi.";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var g = await _context.Gorevler
                .Include(x => x.Islem)
                .Include(x => x.Personel)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id.Value);

            if (g == null) return NotFound();

            ViewData["Title"] = "Görev Sil";
            return View(g);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var g = await _context.Gorevler.FindAsync(id);
            if (g != null)
            {
                _context.Gorevler.Remove(g);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Görev silindi.";
            }
            else
            {
                TempData["Error"] = "Görev bulunamadı.";
            }

            return RedirectToAction(nameof(Index));
        }

        // Zaten atanmışsa yeniden atama yok; değilse sistem atar
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AutoAssign(int id)
        {
            var alreadyAssigned = await _context.Gorevler
                .Where(g => g.Id == id)
                .Select(g => g.PersonelId)
                .FirstOrDefaultAsync();

            if (alreadyAssigned != null)
            {
                TempData["Error"] = "Bu görev zaten atanmış.";
                return RedirectToAction(nameof(Details), new { id });
            }

            await _assignment.AssignTaskAsync(id);

            var g = await _context.Gorevler
                .Include(x => x.Personel)
                .AsNoTracking()
                .FirstAsync(x => x.Id == id);

            TempData["Success"] =
                $"Görev otomatik olarak \"{g.Personel?.Ad} {g.Personel?.Soyad}\" kişisine atandı.";

            return RedirectToAction(nameof(Details), new { id });
        }

        private async Task FillDropdowns(
            int? selectedPersonelId = null,
            int? selectedIslemId = null)
        {
            ViewData["PersonelId"] = new SelectList(
                await _context.Personeller.AsNoTracking()
                    .OrderBy(p => p.Ad)
                    .ThenBy(p => p.Soyad)
                    .Select(p => new { p.Id, FullName = p.Ad + " " + p.Soyad })
                    .ToListAsync(),
                "Id", "FullName", selectedPersonelId);

            ViewData["IslemId"] = new SelectList(
                await _context.Islemler.AsNoTracking()
                    .OrderBy(i => i.Zorluk)
                    .ThenBy(i => i.Ad)
                    .ToListAsync(),
                "Id", "Ad", selectedIslemId);
        }
    }
}
