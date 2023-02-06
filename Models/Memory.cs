namespace BulkyBookWeb.Models
{
    public class Memory : IStorage
    {
        public bool CanRead { get; set; } = true;
        public bool CanWrite { get; set; } = true;
    }
}
