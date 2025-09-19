using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace BursaFuarMerkezi.Models
{
    public class Slider
    {
        [Key]
        public int Id { get; set; }


        [ValidateNever]
        public string? ImageUrl { get; set; }

        public int DisplayOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;
    }
}


