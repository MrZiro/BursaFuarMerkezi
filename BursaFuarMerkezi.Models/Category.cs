using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BursaFuarMerkezi.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1,100,ErrorMessage = "Ekran siparişi 1-100 arasında olmalıdır")]
        public int DisplayOrder { get; set; }

    }
}
