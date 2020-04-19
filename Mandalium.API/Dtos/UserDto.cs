using System;
using System.ComponentModel.DataAnnotations;

namespace Mandalium.API.Dtos
{
    public class UserDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Surname { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Background { get; set; }
        public string PhotoUrl {get; set;}
    }
}