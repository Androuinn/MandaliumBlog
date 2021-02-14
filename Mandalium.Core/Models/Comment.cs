using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mandalium.Core.Models
{
    public class Comment
    {
     
        public Guid Id { get; set; }
        public string CommenterName { get; set; }
        public string Email { get; set; }
        public string CommentString { get; set; }
        public DateTime CreatedDate { get; set; }
        public BlogEntry BlogEntry { get; set; }
        public User User {get;set;}


    }
}