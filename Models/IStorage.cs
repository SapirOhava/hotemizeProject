using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBookWeb.Models
{
    public interface IStorage
    {
        [Required]
        public bool CanRead { get; set; }
        [Required]
        public bool CanWrite { get; set; }
        [Required]
        public bool IsExpired { get; set; }
        [Required]
        public int HoursUntilExpire { get; set; }
        [Required]
        public bool IsInserted { get; set; }
        public DateTime LastUpdatedTime { get; set; }
    }
}