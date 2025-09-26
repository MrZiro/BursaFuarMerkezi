namespace BursaFuarMerkezi.Models.ViewModels
{
    public class DashboardStatsVM
    {
        public int TotalBlogs { get; set; }
        public int TotalFuars { get; set; }
        public int TotalContacts { get; set; }
        public int TotalStandRequests { get; set; }
        public int TotalSectors { get; set; }
        
        public IEnumerable<Blog> RecentBlogs { get; set; } = new List<Blog>();
        public IEnumerable<Contact> RecentContacts { get; set; } = new List<Contact>();
        public IEnumerable<StandRequest> RecentStandRequests { get; set; } = new List<StandRequest>();
    }
}
