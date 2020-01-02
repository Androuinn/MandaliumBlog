using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mandalium.API.Models
{
    public class Writer
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Background { get; set; }


       
    }
}