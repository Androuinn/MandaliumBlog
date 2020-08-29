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
        public async Task<IActionResult> GetUsers()
        {
            var writers = _mapper.Map<IEnumerable<UserDto>>(await _repo.GetUsers());

            foreach (var item in writers)
            {
                item.PhotoUrl = _cloudinary.Api.UrlImgUp.Secure().Transform(new Transformation().Height(500).Crop("scale")).BuildUrl(item.PhotoUrl + ".webp");
            }
            return Ok(writers);
        }

        [Route("[action]")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var user = _mapper.Map<UserDto>(await _repo.GetUser(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)));

            if (!string.IsNullOrEmpty(user.PhotoUrl))
            {
                 user.PhotoUrl = _cloudinary.Api.UrlImgUp.Secure().Transform(new Transformation().Height(500).Crop("scale")).BuildUrl(user.PhotoUrl + ".webp");
            }
           
            return Ok(user);
        }

        [Route("[action]")]
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody]UserDto userDto)
        {
            if (int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) != userDto.Id)
            {
                return Unauthorized();
            }


            var user = _mapper.Map<User>(userDto);

            await _repo.UpdateUser(user);

            return StatusCode(200);
        }


    }
}