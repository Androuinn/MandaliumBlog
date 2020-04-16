using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Mandalium.API.Data;
using Mandalium.API.Dtos;
using Mandalium.API.Helpers;
using Mandalium.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Security.Claims;

namespace Mandalium.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> cloudinaryConfig;
        private Cloudinary _cloudinary;
        private readonly IUserRepository _repo;
        public UserController(IUserRepository repo, IMapper mapper, IOptions<CloudinarySettings> _cloudinaryConfig)
        {
            this._repo = repo;
            this.cloudinaryConfig = _cloudinaryConfig;
            this._mapper = mapper;

            Account acc = new Account(
                 _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetWriters()
        {
            var writers = _mapper.Map<IEnumerable<WriterDto>>(await _repo.GetWriters());

            foreach (var item in writers)
            {
                item.PhotoUrl = _cloudinary.Api.UrlImgUp.Secure().Transform(new Transformation().Height(500).Crop("scale")).BuildUrl(item.PhotoUrl + ".webp");
            }
            return Ok(writers);
        }

        [Route("[action]")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetWriter()
        {
            var writer = _mapper.Map<WriterDto>(await _repo.GetWriter(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)));

            writer.PhotoUrl = _cloudinary.Api.UrlImgUp.Secure().Transform(new Transformation().Height(500).Crop("scale")).BuildUrl(writer.PhotoUrl + ".webp");
            return Ok(writer);
        }

        [Route("[action]")]
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateWriter([FromBody]WriterDto writerDto)
        {
            if (int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) != writerDto.Id)
            {
                return Unauthorized();
            }


            var writer = _mapper.Map<Writer>(writerDto);

            await _repo.UpdateWriter(writer);

            return StatusCode(200);
        }


    }
}