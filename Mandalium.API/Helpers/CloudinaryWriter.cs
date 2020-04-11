using System;
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

        // public static IEnumerable<T> CloudinaryImageConvert<T>(IEnumerable<T> list, int height)
        // {
        //     foreach (T item in list)
        //     {
        //         Type.GetType("item").GetProperty("PhotoUrl").SetValue(_cloudinary.Api.UrlImgUp.Secure().Transform(new Transformation().Height(height).Crop("scale")).BuildUrl(Type.GetType("T").GetProperty("PhotoUrl").GetValue().Split("/").Last().Split(".").First() + ".webp"));
          
        //     }
        //     return list;
        // }

    }
}