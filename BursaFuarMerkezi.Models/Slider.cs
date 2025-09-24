using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BursaFuarMerkezi.Models
{
    public class Slider
    {
        [Key]
        public int Id { get; set; }

        // Multilingual fields - Turkish
        [MaxLength(200)]
        [Display(Name = "Title (Turkish)")]
        public string? TitleTr { get; set; }

        [MaxLength(500)]
        [Display(Name = "Description (Turkish)")]
        public string? DescriptionTr { get; set; }

        [MaxLength(100)]
        [Display(Name = "Button Text (Turkish)")]
        public string? ButtonTextTr { get; set; }

        // Multilingual fields - English
        [MaxLength(200)]
        [Display(Name = "Title (English)")]
        public string? TitleEn { get; set; }

        [MaxLength(500)]
        [Display(Name = "Description (English)")]
        public string? DescriptionEn { get; set; }

        [MaxLength(100)]
        [Display(Name = "Button Text (English)")]
        public string? ButtonTextEn { get; set; }

        // Image fields
        [Required]
        [Display(Name = "Desktop Image URL")]
        public string ImageUrl { get; set; } = string.Empty;

        [Display(Name = "Mobile Image URL")]
        public string? MobileImageUrl { get; set; }

        [MaxLength(200)]
        [Display(Name = "Alt Text")]
        public string? AltText { get; set; }

        // Link and behavior
        [MaxLength(500)]
        [Display(Name = "Link URL")]
        public string? LinkUrl { get; set; }

        [Display(Name = "Open in New Tab")]
        public bool OpenInNewTab { get; set; } = false;

        // Display settings
        [Required]
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; } = 1;

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        // Audit fields
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties for file uploads (not mapped)
        [NotMapped]
        [ValidateNever]
        public IFormFile? ImageFile { get; set; }

        [NotMapped]
        [ValidateNever]
        public IFormFile? MobileImageFile { get; set; }
    }
}