namespace BulkyBookWeb.Models
{
    public class Memory : IStorage
    {
        public bool CanRead { get; set; } = true;
        public bool CanWrite { get; set; } = true;
        public bool IsExpired { get; set; } = true;
        public int HoursUntilExpire { get; set; } = 1;
    }
}
