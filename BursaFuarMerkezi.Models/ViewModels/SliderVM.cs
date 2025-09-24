using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BursaFuarMerkezi.Models.ViewModels
{
    public class SliderVM
    {
        public Slider Slider { get; set; } = new();

        [ValidateNever]
        public IFormFile? DesktopImage { get; set; }
        
        [ValidateNever]
        public IFormFile? MobileImage { get; set; }
    }
}