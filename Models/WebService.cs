namespace BulkyBookWeb.Models
{
    public class WebService : IStorage
    {
        public bool CanRead { get; set; } = true;
        public bool CanWrite { get; set; } = false;

    }
}
