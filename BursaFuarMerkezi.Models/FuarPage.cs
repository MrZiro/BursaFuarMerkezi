using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BursaFuarMerkezi.Models
{
    public class FuarPage
    {
        [Key]
        public int Id { get; set; }

        // Multilingual fields
        [Required]
        [MaxLength(100)]
        public string TitleTr { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string TitleEn { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string SubTitleTr { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string SubTitleEn { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? SlugTr { get; set; }
        [MaxLength(100)]
        public string? SlugEn { get; set; }

        [ValidateNever]
        public string? ContentTr { get; set; } = string.Empty;
        [ValidateNever]
        public string? ContentEn { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }



        // New fields for Fuar Künyesi (Fair Information)
        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(3);

        [MaxLength(50)]
        public string FairHall { get; set; } = "1";

        [MaxLength(100)]
        public string OrganizerTr { get; set; } = string.Empty;
        [MaxLength(100)]
        public string OrganizerEn { get; set; } = string.Empty;

        [MaxLength(50)]
        public string VisitingHours { get; set; } = "09:30-18:00";

        [MaxLength(200)]
        public string FairLocationTr { get; set; } = string.Empty;
        [MaxLength(200)]
        public string FairLocationEn { get; set; } = string.Empty;
        [ValidateNever]
        public ICollection<Sector> Sectors { get; set; } = new List<Sector>();

        [MaxLength(200)]
        public string WebsiteUrl { get; set; } = string.Empty;

        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Sector { get; set; } = string.Empty;

        [ValidateNever]
        public string? FeaturedImageUrl { get; set; } 

        [ValidateNever]
        public string? CardImageUrl { get; set; }

        public bool IsPublished { get; set; } = true;
        [MaxLength(500)]
        public string MetaDescription { get; set; } = string.Empty;

        [MaxLength(100)]
        public string MetaKeywords { get; set; } = string.Empty;

        //[MaxLength(50)]
        //public string PageType { get; set; } = "fuar"; // Default type


    }
}
