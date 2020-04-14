using System;
using System.ComponentModel.DataAnnotations;

namespace Mandalium.API.Dtos
{
    public class WriterForRegisterDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        public string Username { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 4, ErrorMessage = "4 ile 16 karakter arasında bir şifre girmelisiniz.")]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Surname { get; set; }
        [Required]
         [MaxLength(100)]
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}