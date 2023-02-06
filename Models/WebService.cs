namespace BulkyBookWeb.Models
{
    public class WebService : IStorage
    {
        public bool CanRead { get; set; } = true;
        public bool CanWrite { get; set; } = false;
        public bool IsExpired { get; set; } = false;
        public bool IsInserted { get; set; } = true;
        public DateTime LastUpdatedTime { get; set; }
        public int HoursUntilExpire { get; set; } = 0;
    }
}
