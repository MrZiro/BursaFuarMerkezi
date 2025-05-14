using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BursaFuarMerkezi.DataAccess.Pagination
{
    public class FuarFilterParams
    {
        private int _pageSize = 10;
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
        /// Filter by year (e.g., "2025")
        /// </summary>
        public string Year { get; set; }
        
        /// <summary>
        /// Filter by month (e.g., "Ocak", "Şubat")
        /// </summary>
        public string Month { get; set; }
        
        /// <summary>
        /// Filter by sector (e.g., "Turizm", "Teknoloji")
        /// </summary>
        public string Sector { get; set; }
        
        /// <summary>
        /// Filter by location/city (e.g., "İstanbul", "Ankara")
        /// </summary>
        public string Location { get; set; }
    }
} 