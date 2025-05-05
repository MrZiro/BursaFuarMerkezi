using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BursaFuarMerkezi.Models
{
    public class FuarPage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(100)]
        public string Slug { get; set; }

        [ValidateNever]
        public string Content { get; set; }

        [ValidateNever]
        public string FeaturedImageUrl { get; set; }

        public bool IsPublished { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }

        [MaxLength(500)]
        public string MetaDescription { get; set; }

        [MaxLength(100)]
        public string MetaKeywords { get; set; }

        [MaxLength(50)]
        public string PageType { get; set; } = "fuar"; // Default type
    }
} 