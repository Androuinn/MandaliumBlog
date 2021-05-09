using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mandalium.API.Models
{
    public class CommentCreateRequest
    {
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string CommenterName { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [MaxLength(500)]
        public string CommentString { get; set; }
        [Required]
        public int BlogEntryId { get; set; }

        public int? userId { get; set; }
    }
}
