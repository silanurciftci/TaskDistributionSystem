using System.ComponentModel.DataAnnotations;

namespace TaskDistributionSystem.Models
{
    public class Personel
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Ad { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Soyad { get; set; } = string.Empty;
    }
}
