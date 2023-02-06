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
    }
}
