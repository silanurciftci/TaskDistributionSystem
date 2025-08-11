using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskDistributionSystem.Models;

namespace TaskDistributionSystem.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly AppDbContext _db;
        public AssignmentService(AppDbContext db) => _db = db;

        private sealed class Candidate
        {
            public int Id { get; set; }
            public int Count { get; set; }   
            public int? LastZ { get; set; }  
        }

       
        public async Task AssignTaskAsync(int gorevId)
        {
            var gorev = await _db.Gorevler
                .Include(g => g.Islem)
                .FirstOrDefaultAsync(g => g.Id == gorevId);

            if (gorev == null || gorev.Islem == null)
                throw new InvalidOperationException("Görev veya İşlem bulunamadı.");

            int hedefZ = gorev.Islem.Zorluk;

            var persons = await _db.Personeller.AsNoTracking().ToListAsync();
            if (persons.Count == 0)
                throw new InvalidOperationException("Atanabilir personel bulunamadı.");

           
            var sameDiffCounts = await _db.Gorevler
                .Where(g => g.PersonelId != null)
                .Join(_db.Islemler, g => g.IslemId, i => i.Id,
                      (g, i) => new { g.PersonelId, i.Zorluk })
                .Where(x => x.Zorluk == hedefZ)
                .GroupBy(x => x.PersonelId!.Value)
                .Select(g => new { PersonelId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.PersonelId, x => x.Count);

           
            var lastPerPerson = await _db.Gorevler
                .Where(g => g.PersonelId != null)
                .OrderByDescending(g => g.Tarih)
                .ThenByDescending(g => g.Id)
                .Join(_db.Islemler, g => g.IslemId, i => i.Id,
                      (g, i) => new { g.PersonelId, Zorluk = i.Zorluk })
                .ToListAsync();

            var lastMap = lastPerPerson
                .GroupBy(x => x.PersonelId!.Value)
                .ToDictionary(g => g.Key, g => g.First().Zorluk);

            
            var candidates = persons
                .Select(p => new Candidate
                {
                    Id    = p.Id,
                    Count = sameDiffCounts.TryGetValue(p.Id, out var c) ? c : 0,
                    LastZ = lastMap.TryGetValue(p.Id, out var z) ? (int?)z : null
                })
                .Where(x => !x.LastZ.HasValue || Math.Abs(x.LastZ.Value - hedefZ) > 1)
                .OrderBy(x => x.Count)  
                .ThenBy(x => x.Id)      
                .ToList();

            if (candidates.Count == 0)
                throw new InvalidOperationException(
                    "Ardışık zorluk kuralı nedeniyle uygun aday bulunamadı.");

            gorev.PersonelId = candidates.First().Id;
            await _db.SaveChangesAsync();
        }
    }
}
