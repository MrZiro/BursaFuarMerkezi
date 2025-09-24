using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BursaFuarMerkezi.DataAccess.Pagination
{
    public class BlogFilterParams
    {
        private int _pageSize = 7;
        private int _pageNumber = 1;
        
        /// <summary>
        /// Current page number (1-based)
        /// </summary>
        public int PageNumber 
        { 
            get => _pageNumber; 
            set => _pageNumber = value < 1 ? 1 : value; 
        }
        
        /// <summary>
        /// Number of items per page
        /// </summary>
        public int PageSize 
        { 
            get => _pageSize; 
            set => _pageSize = value < 1 ? 1 : value; 
        }
        
        /// <summary>
        /// Filter by category (e.g., "Gündem", "Blog", "Duyurular", "Haberler")
        /// </summary>
        public string? Category { get; set; } = "all";
    }
}
