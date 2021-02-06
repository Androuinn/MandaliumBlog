using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mandalium.Core.Dto
{
    public class BlogEntryListDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(200)")]
        public string Headline { get; set; }

        [Required]
        [Column(TypeName = "varchar(500)")]
        public string SubHeadline { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string PhotoUrl { get; set; }

        public DateTime CreatedDate { get; set; }
        // public int TimesRead { get; set; }
        // public string WriterName { get; set; }
        // public string WriterSurname { get; set; }

        public string TopicName { get; set; }

        public string WriterName { get; set; }
        public string WriterSurname { get; set; }
    }
}