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
        [DisplayName("Content Type")]
        public string Name { get; set; }
    }
} 