﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mandalium.API.Dtos
{
    public class BlogEntryForCreationDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(200)")]
        public string Headline { get; set; }

        [Required]
        [Column(TypeName = "varchar(500)")]
        public string SubHeadline { get; set; }

        [Required]
        [Column(TypeName = "varchar(MAX)")]
        public string innerTextHtml { get; set; }
       
        [Column(TypeName = "varchar(250)")]
        public string PhotoUrl { get; set; }
        public bool WriterEntry {get; set;}

        public int WriterId { get; set; }
       
        public int TopicId { get; set; }
     
    }
}