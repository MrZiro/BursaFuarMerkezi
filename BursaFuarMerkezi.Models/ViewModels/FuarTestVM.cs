using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BursaFuarMerkezi.Models.ViewModels
{
    public class FuarTestVM
    {
        public FuarTest FuarTest { get; set; }


        [ValidateNever]
        public IFormFile? FeaturedImage { get; set; }

        [ValidateNever]
        public IFormFile? CardImage { get; set; }
    }
}
