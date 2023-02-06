namespace BulkyBookWeb.Models
{
    public class FileSystem : IStorage
    {
        public bool CanRead { get; set; } = true;
        public bool CanWrite { get; set; } = true;

    }
}
