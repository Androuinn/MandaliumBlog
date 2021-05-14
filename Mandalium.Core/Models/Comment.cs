using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mandalium.Core.Models
{
    public class Comment : BaseEntityWithId
    {
        public string CommenterName { get; set; }
        public string Email { get; set; }
        public string CommentString { get; set; }
        public DateTime CreatedOn { get; set; }
        public BlogEntry BlogEntry { get; set; }
        public User User {get;set;}


    }
}