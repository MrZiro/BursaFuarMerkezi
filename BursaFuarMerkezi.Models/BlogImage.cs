using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BursaFuarMerkezi.Models
{
    public class BlogImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BlogId { get; set; }

        [ForeignKey("BlogId")]
        [ValidateNever]
        public Blog Blog { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Caption { get; set; }

        public int DisplayOrder { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}




