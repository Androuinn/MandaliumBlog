using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mandalium.API.Models
{
    public class Photo
    {
        public int Id { get; set; } 
        [Required]
         [Column(TypeName="varchar(300)")]
        public string PhotoUrl { get; set; }
        [Required]
        [Column(TypeName="varchar(150)")]
        public string PublicId { get; set; }
        public int? UserId { get; set; }
        public User  User { get; set; }

    }
}