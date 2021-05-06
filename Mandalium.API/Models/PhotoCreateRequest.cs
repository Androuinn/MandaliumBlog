using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mandalium.API.Models
{
    public class PhotoCreateRequest
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; }
        public string PublicId { get; set; }
        public int UserId { get; set; }
        public IFormFile File { get; set; }

    }
}
