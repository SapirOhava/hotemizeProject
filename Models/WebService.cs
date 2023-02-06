namespace BulkyBookWeb.Models
{
    public class WebService : IStorage
    {
        public bool CanRead { get; set; } = true;
        public bool CanWrite { get; set; } = false;
        public bool IsExpired { get; set; } = false;

        public int HoursUntilExpire { get; set; } = Int32.MaxValue;
    }
}
