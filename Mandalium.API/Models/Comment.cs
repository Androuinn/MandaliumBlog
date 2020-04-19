using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mandalium.API.Models
{
    public class Comment
    {
        [Required]
        public Guid Id { get; set; }
       
        [Column(TypeName = "varchar(100)")]
        public string CommenterName { get; set; }
        
        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }
        [Required]
        [Column(TypeName = "varchar(500)")]
        public string CommentString { get; set; }
        public DateTime CreatedDate { get; set; }

        [Required]
        public int BlogEntryId { get; set; }
        public BlogEntry BlogEntry { get; set; }


        public int? UserId { get; set; }
        public User User {get;set;}


        public Comment()
        {
            Id = new Guid();
            CreatedDate = DateTime.Now;
        }
    }
}