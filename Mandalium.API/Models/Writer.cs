using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mandalium.API.Models
{
    public class Writer
    {
        [Required]
        public int Id { get; set; }


        [Required]
        [Column(TypeName = "varchar(50)")]
        [MinLength(5)]
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }



        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Surname { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Background { get; set; }

        [Column(TypeName = "varchar(300)")]
        public string PhotoUrl { get; set; }

        public bool Role { get; set; } = false;





    }
}