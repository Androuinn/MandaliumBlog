using System.Collections.Generic;
using System.Linq;
using CloudinaryDotNet;
using Mandalium.API.Models;
using Microsoft.Extensions.Options;

namespace Mandalium.API.Helpers
{
    public  class CloudinaryWriter
    {
         private readonly IOptions<CloudinarySettings> cloudinaryConfig;
        private static Cloudinary _cloudinary;
        public CloudinaryWriter(IOptions<CloudinarySettings> _cloudinaryConfig)
        {
            this.cloudinaryConfig = _cloudinaryConfig;
            
             Account acc = new Account(
                 _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }

        // public static List<T> CloudinaryImageConvert<T>(List<T> list, int height)
        // {
        //     foreach (BlogEntry item in list)
        //     {
        //          item.PhotoUrl = _cloudinary.Api.UrlImgUp.Secure().Transform(new Transformation().Height(500).Crop("scale")).BuildUrl(item.PhotoUrl.Split("/").Last().Split(".").First() + ".webp");
          
        //     }
        // }

    }
}