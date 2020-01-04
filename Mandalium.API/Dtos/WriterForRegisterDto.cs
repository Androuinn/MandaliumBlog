using System;
using System.ComponentModel.DataAnnotations;

namespace Mandalium.API.Dtos
{
    public class WriterForRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 4, ErrorMessage = "You must specify password between 4 and 8 characters")]
        public string Password { get; set; }

        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
    }
}