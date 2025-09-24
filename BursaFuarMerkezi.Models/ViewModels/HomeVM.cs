using System.Collections.Generic;
using BursaFuarMerkezi.Models;

namespace BursaFuarMerkezi.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Slider> Sliders { get; set; } = new List<Slider>();
    }
}
