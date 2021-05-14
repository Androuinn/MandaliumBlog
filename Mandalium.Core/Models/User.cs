using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mandalium.Core.Models
{
    public class User : BaseEntityWithId
    {

        [MinLength(5)]
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Background { get; set; }
        public string PhotoUrl { get; set; }
        public int? ActivationPin { get; set; }
        public bool IsActivated { get; set; }
        public DateTime CreatedOn { get; set; }

        public bool Role { get; set; } = false;



        public ICollection<Comment> Comments { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<BlogEntry>  BlogEntries { get; set; } 

    }
}