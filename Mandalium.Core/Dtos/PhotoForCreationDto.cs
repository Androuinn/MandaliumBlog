using Microsoft.AspNetCore.Http;

namespace Mandalium.Core.Dto
{
    public class PhotoForCreationDto
    {
        public int Id { get; set; } 
        public string PhotoUrl { get; set; }
        public string PublicId { get; set; }
        public int UserId { get; set; }
        public IFormFile File { get; set; }
    }
}