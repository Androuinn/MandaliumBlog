using System;
using System.ComponentModel.DataAnnotations;

namespace Mandalium.API.Dtos
{
    public class CommentDtoForCreation
    {
       
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string CommenterName { get; set; }
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [MaxLength(500)]
        public string CommentString { get; set; }
        [Required]
        public int BlogEntryId { get; set; }

    }
}