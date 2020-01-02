using System;
using System.ComponentModel.DataAnnotations;

namespace Mandalium.API.Dtos
{
    public class CommentDto
    {
       
        [Required]
        [MaxLength(100)]
        public string CommenterName { get; set; }
        [Required]
        [MaxLength(500)]
        public string CommentString { get; set; }
        public DateTime CreatedDate { get; set; }
       
    }
}