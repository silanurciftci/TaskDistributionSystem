using System.ComponentModel.DataAnnotations;

namespace TaskDistributionSystem.Models
{
    public class Islem
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Ad { get; set; } = string.Empty;

        [Range(1, 8)]
        public int Zorluk { get; set; }
    }
}
