using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BursaFuarMerkezi.Models.ViewModels
{
    public class SliderVM
    {
        public Slider Slider { get; set; }

        [ValidateNever]
        public IFormFile? Image { get; set; }
    }
}


