﻿﻿using Microsoft.AspNetCore.Http;
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
    public class Blog
    {
        [Key]
        public int Id { get; set; }

        // Multilingual fields - Turkish
        [Required]
        [MaxLength(200)]
        [Display(Name = "Title (Turkish)")]
        public string TitleTr { get; set; } = string.Empty;

        [MaxLength(200)]
        [Display(Name = "Slug (Turkish)")]
        public string SlugTr { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Content (Turkish)")]
        public string ContentTr { get; set; } = string.Empty;

        [Display(Name = "Short Description (Turkish)")]
        [MaxLength(500)]
        public string ShortDescriptionTr { get; set; } = string.Empty;

        // Multilingual fields - English
        [Required]
        [MaxLength(200)]
        [Display(Name = "Title (English)")]
        public string TitleEn { get; set; } = string.Empty;

        [MaxLength(200)]
        [Display(Name = "Slug (English)")]
        public string SlugEn { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Content (English)")]
        public string ContentEn { get; set; } = string.Empty;

        [Display(Name = "Short Description (English)")]
        [MaxLength(500)]
        public string ShortDescriptionEn { get; set; } = string.Empty;


        [ValidateNever]

        [Display(Name = "Featured Image")]
        [Required]
        public string CardImageUrl { get; set; } = string.Empty;

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Updated At")]
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Is Published")]
        public bool IsPublished { get; set; } = true;



        // SEO fields - Turkish
        [Display(Name = "Meta Description (Turkish)")]
        [MaxLength(200)]
        public string MetaDescriptionTr { get; set; } = string.Empty;

        [Display(Name = "Meta Keywords (Turkish)")]
        [MaxLength(200)]
        public string MetaKeywordsTr { get; set; } = string.Empty;

        // SEO fields - English
        [Display(Name = "Meta Description (English)")]
        [MaxLength(200)]
        public string MetaDescriptionEn { get; set; } = string.Empty;

        [Display(Name = "Meta Keywords (English)")]
        [MaxLength(200)]
        public string MetaKeywordsEn { get; set; } = string.Empty;



        // Foreign key
        [Display(Name = "Category")]
        public int? ContentTypeId { get; set; }

        [ForeignKey("ContentTypeId")]
        [ValidateNever]
        public ContentType ContentType { get; set; }


    }
}
