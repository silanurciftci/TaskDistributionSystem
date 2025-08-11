using System.ComponentModel.DataAnnotations;

namespace TaskDistributionSystem.Models
{
    public class Developer
    {
        public int Id { get; set; }

        [Required] public string Ad { get; set; } = string.Empty;
        [Required] public string Soyad { get; set; } = string.Empty;
    }
}
