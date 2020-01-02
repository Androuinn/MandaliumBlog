﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mandalium.API.Models
{
    public class BlogEntry
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
        public DateTime CreatedDate { get; set; }
        public int TimesRead { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string PhotoUrl { get; set; }
        public bool WriterEntry { get; set; }


        public int? WriterId { get; set; }
        public Writer Writer { get; set; }

        public int? TopicId { get; set; }
        public Topic Topic { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public BlogEntry()
        {
            CreatedDate = DateTime.Now;
            TimesRead = 0;
            WriterEntry = false;
        }

    }
}