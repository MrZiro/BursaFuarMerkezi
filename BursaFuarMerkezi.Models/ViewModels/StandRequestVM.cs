using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BursaFuarMerkezi.Models.ViewModels
{
    public class StandRequestVM
    {
        public string Exhibition { get; set; }
        public string Company { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> FuarList { get; set; }
    }
}
