using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskDistributionSystem.Models
{
    public class Gorev
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Ad { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime Tarih { get; set; } = DateTime.Today;

        [MaxLength(1000)]
        public string? Aciklama { get; set; }

        [ForeignKey(nameof(Islem))]
        public int IslemId { get; set; }
        public Islem? Islem { get; set; }

        [ForeignKey(nameof(Personel))]
        public int? PersonelId { get; set; }
        public Personel? Personel { get; set; }
    }
}
