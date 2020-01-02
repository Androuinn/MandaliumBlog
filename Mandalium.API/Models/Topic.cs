using System.ComponentModel.DataAnnotations;

namespace Mandalium.API.Models
{
    public class Topic
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string TopicName { get; set; }
    }
}