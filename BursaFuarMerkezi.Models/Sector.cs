using System.ComponentModel.DataAnnotations;

namespace BursaFuarMerkezi.Models
{
    public class Sector
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string NameTr { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string NameEn { get; set; } = string.Empty;
    }
}


