using Microsoft.EntityFrameworkCore;

namespace TaskDistributionSystem.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Personel> Personeller => Set<Personel>();
        public DbSet<Islem>    Islemler    => Set<Islem>();
        public DbSet<Gorev>    Gorevler    => Set<Gorev>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // İlişkiler
            modelBuilder.Entity<Gorev>()
                .HasOne(g => g.Personel).WithMany()
                .HasForeignKey(g => g.PersonelId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Gorev>()
                .HasOne(g => g.Islem).WithMany()
                .HasForeignKey(g => g.IslemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed: 8 Personel
            modelBuilder.Entity<Personel>().HasData(
                new Personel { Id = 1, Ad = "Ali",    Soyad = "Yılmaz" },
                new Personel { Id = 2, Ad = "Ayşe",   Soyad = "Demir"  },
                new Personel { Id = 3, Ad = "Mehmet", Soyad = "Kaya"   },
                new Personel { Id = 4, Ad = "Elif",   Soyad = "Çetin"  },
                new Personel { Id = 5, Ad = "Can",    Soyad = "Aksoy"  },
                new Personel { Id = 6, Ad = "Zeynep", Soyad = "Şahin"  },
                new Personel { Id = 7, Ad = "Burak",  Soyad = "Koç"    },
                new Personel { Id = 8, Ad = "Ece",    Soyad = "Aydın"  }
            );

            // Seed: 8 İşlem (Zorluk 1..8)
            modelBuilder.Entity<Islem>().HasData(
                new Islem { Id = 1, Ad = "Talep İnceleme",      Zorluk = 1 },
                new Islem { Id = 2, Ad = "Hata Düzeltme",       Zorluk = 2 },
                new Islem { Id = 3, Ad = "Küçük Geliştirme",    Zorluk = 3 },
                new Islem { Id = 4, Ad = "Orta Geliştirme",     Zorluk = 4 },
                new Islem { Id = 5, Ad = "Büyük Geliştirme",    Zorluk = 5 },
                new Islem { Id = 6, Ad = "Kod İnceleme",        Zorluk = 6 },
                new Islem { Id = 7, Ad = "Refactor",            Zorluk = 7 },
                new Islem { Id = 8, Ad = "Performans İyileşt.", Zorluk = 8 }
            );
        }
    }
}
