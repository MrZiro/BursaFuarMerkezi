using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BursaFuarMerkezi.Models
{
    public class ContentType
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        [DisplayName("Content Type (Turkish)")]
        public string NameTr { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        [DisplayName("Content Type (English)")]
        public string NameEn { get; set; } = string.Empty;
    }
} 