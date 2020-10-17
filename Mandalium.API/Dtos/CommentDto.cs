using System;
using System.ComponentModel.DataAnnotations;

namespace Mandalium.API.Dtos
{
    public class CommentDto
    {
        public int CommenterId { get; set; }
        [MaxLength(100)]
        public string CommenterName { get; set; }
        public string PhotoUrl { get; set; }
        
        [MaxLength(500)]
        public string CommentString { get; set; }
        public DateTime CreatedDate { get; set; }

       
       
    }
}