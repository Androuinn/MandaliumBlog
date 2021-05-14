using System.ComponentModel.DataAnnotations;

namespace Mandalium.Core.Models
{
    public class Topic : BaseEntityWithId
    {
       
        public string TopicName { get; set; }
    }

   
}