using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BursaFuarMerkezi.Models.ViewModels
{
    public class FuarPageVM
    {
        public FuarPage FuarPage { get; set; }
        
        [ValidateNever]
        public IFormFile FeaturedImage { get; set; }
    }
} 