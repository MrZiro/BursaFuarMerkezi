using System;
using System.ComponentModel.DataAnnotations;

namespace BursaFuarMerkezi.Models
{
    public class StandRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Fuar Seçiniz")]
        public string Exhibition { get; set; }

        [Required]
        [Display(Name = "Firma Ünvanı")]
        public string Company { get; set; }

        [Required]
        [Display(Name = "Adınız")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Soyadınız")]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-posta Adresiniz")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Cep Telefonu Numaranız")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Talep/ Soru/ Görüş")]
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsEmailSent { get; set; }
    }
}
