using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mandalium.Core.Models
{
    public class Photo
    {
        public int Id { get; set; } 
        public string PhotoUrl { get; set; }
        public string PublicId { get; set; }
        public User  User { get; set; }

    }
}