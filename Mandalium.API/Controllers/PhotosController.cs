using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Mandalium.API.Data;
using Mandalium.API.Dtos;
using Mandalium.API.Helpers;
using Mandalium.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Mandalium.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhotosController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IPhotoRepository _repo;

        private readonly IOptions<CloudinarySettings> cloudinaryConfig;
        private Cloudinary _cloudinary;
        public PhotosController(IOptions<CloudinarySettings> _cloudinaryConfig, IMapper mapper, IPhotoRepository repo)
        {
            this._repo = repo;
            this._mapper = mapper;
            this.cloudinaryConfig = _cloudinaryConfig;
            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostPhoto([FromForm]PhotoForCreationDto photoForCreationDto)
        {
            if (photoForCreationDto.WriterId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) || User.FindFirst(ClaimTypes.Role).Value != "1")
            {
                return Unauthorized();
            }

            var uploadResult = new ImageUploadResult();
            if (photoForCreationDto.File.Length > 0)
            {

                using (var stream = photoForCreationDto.File.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(photoForCreationDto.File.Name, stream),
                        Folder = "Writers/" + photoForCreationDto.WriterId
                    };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);

                }
            }

            if (uploadResult != null)
            {
                photoForCreationDto.WriterId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                photoForCreationDto.PhotoUrl = uploadResult.Uri.ToString();
                photoForCreationDto.PublicId = uploadResult.PublicId;

                await _repo.AddPhoto(_mapper.Map<Photo>(photoForCreationDto));

                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(photoForCreationDto.PublicId));
            }
            return BadRequest();
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPhoto([FromQuery]UserParams userParams)
        {
            if (userParams.WriterId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) || User.FindFirst(ClaimTypes.Role).Value != "1")
            {
                return Unauthorized();
            }

            var photos = await _repo.GetPhotos(userParams);
            var dtos = _mapper.Map<IEnumerable<PhotoDto>>(photos);

            foreach (var item in dtos)
            {
                item.PublicId = _cloudinary.Api.UrlImgUp.Secure().Transform(new Transformation().Height(250).Crop("scale")).BuildUrl(item.PublicId + ".webp");
            }

            Response.AddPagination(photos.CurrentPage, photos.PageSize, photos.TotalCount, photos.TotalPages);

            return Ok(dtos);
        }

    }
}