using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mandalium.API.Models
{
    public class SystemSetting
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(200)")]
        public string Key { get; set; }
        [Required]
        [Column(TypeName = "varchar(200)")]
        public string Value { get; set; }
    }
}