using System.Collections.Generic;

namespace BursaFuarMerkezi.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<ContentType> ContentTypes { get; set; } = new List<ContentType>();
        public IEnumerable<Blog> LatestBlogs { get; set; } = new List<Blog>();
    }
}
