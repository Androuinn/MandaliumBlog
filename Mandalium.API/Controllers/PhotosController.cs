using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Mandalium.Core.Helpers;
using Mandalium.Core.Dto;
using Mandalium.Core.Helpers.Pagination;
using Mandalium.Core.Interfaces;
using Mandalium.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Mandalium.API.Models;

namespace Mandalium.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhotosController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IPhotoRepository _repo;
        private readonly IUserRepository _userRepo;

        private readonly IOptions<CloudinarySettings> cloudinaryConfig;
        private Cloudinary _cloudinary;
        public PhotosController(IOptions<CloudinarySettings> _cloudinaryConfig, IMapper mapper, IPhotoRepository repo, IUserRepository userRepo)
        {
            this._userRepo = userRepo;
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
        public async Task<IActionResult> PostPhoto([FromForm] PhotoCreateRequest request)
        {
            if (request.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) || User.FindFirst(ClaimTypes.Role).Value != "1")
                return Unauthorized();

            User user = await _userRepo.GetUser(request.UserId);
            if (user == null)
                return Unauthorized();

            var uploadResult = new ImageUploadResult();
            if (request.File.Length > 0)
            {

                using (var stream = request.File.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(request.File.Name, stream),
                        Folder = "Writers/" + request.UserId
                    };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
            }
            try
            {
                if (uploadResult != null)
                {
                    var photo = new Photo()
                    {
                        PhotoUrl = uploadResult.Url.ToString(),
                        PublicId = uploadResult.PublicId,
                        User = user
                    };
                    await _repo.AddPhoto(photo);
                    return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(photo.PublicId));
                }
            }
            catch (System.Exception ex)
            {
                Extensions.ReportError(ex);
                throw;
            }

            return BadRequest();
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPhoto([FromQuery] UserParams userParams)
        {
            if (userParams.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) || User.FindFirst(ClaimTypes.Role).Value != "1")
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


        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateProfilePhoto([FromForm] PhotoCreateRequest request)
        {
            if (request.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userProfile = await _userRepo.GetUser(request.UserId);
            if (userProfile == null)
                return Unauthorized();

            if (userProfile.PhotoUrl != null)
            {
                var deletionParams = new DeletionParams(userProfile.PhotoUrl)
                {
                    PublicId = userProfile.PhotoUrl.ToString()
                };

                if (deletionParams.PublicId != "" && deletionParams.PublicId != null)
                {
                    var deletionResult = _cloudinary.DestroyAsync(deletionParams);
                }
            }


            var uploadResult = new ImageUploadResult();
            if (request.File.Length > 0)
            {

                using (var stream = request.File.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(request.File.Name, stream),
                        Folder = "Writers/" + request.UserId
                    };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);

                }
            }

            if (uploadResult != null)
            {
                request.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                request.PhotoUrl = uploadResult.Url.ToString();
                request.PublicId = uploadResult.PublicId;

                userProfile.PhotoUrl = uploadResult.PublicId;
                await _userRepo.UpdateUser(userProfile);
                request.PublicId = _cloudinary.Api.UrlImgUp.Secure().Transform(new Transformation().Height(500).Crop("scale")).BuildUrl(uploadResult.PublicId + ".webp");
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(request.PublicId));
            }
            return BadRequest();
        }

    }
}