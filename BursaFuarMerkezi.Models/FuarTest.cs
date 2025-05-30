﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BursaFuarMerkezi.Models
{
    public class FuarTest
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
        public string? Content { get; set; } = "content";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }



        // New fields for Fuar Künyesi (Fair Information)
        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(3);

        [MaxLength(50)]
        public string FairHall { get; set; } = "1";

        [MaxLength(100)]
        public string Organizer { get; set; } = "Bursa Fuar Merkezi";

        [MaxLength(50)]
        public string VisitingHours { get; set; } = "09:30-18:00";

        [MaxLength(200)]
        public string FairLocation { get; set; } = "Bursa Uluslararası Fuar ve Kongre Merkezi";

        [MaxLength(200)]
        public string WebsiteUrl { get; set; } = "https://bursafuarmerkezi.com";

        [MaxLength(100)]
        public string City { get; set; } = "Bursa";

        [MaxLength(50)]
        public string Sector { get; set; } = "Turizm";

        [ValidateNever]
        public string? FeaturedImageUrl { get; set; } 

        [ValidateNever]
        public string? CardImageUrl { get; set; }

        public bool IsPublished { get; set; } = true;
        [MaxLength(500)]
        public string MetaDescription { get; set; }

        [MaxLength(100)]
        public string MetaKeywords { get; set; }

        //[MaxLength(50)]
        //public string PageType { get; set; } = "fuar"; // Default type


    }
}
