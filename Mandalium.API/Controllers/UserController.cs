using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Security.Claims;
using Mandalium.Core.Models;
using Mandalium.Core.Interfaces;
using Mandalium.Core.Dto;
using Mandalium.Core.Helpers;

namespace Mandalium.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> cloudinaryConfig;
        private Cloudinary _cloudinary;
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork, IMapper mapper, IOptions<CloudinarySettings> _cloudinaryConfig)
        {
            _unitOfWork = unitOfWork;
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
            var writers = _mapper.Map<IEnumerable<UserDto>>(await _unitOfWork.GetRepository<User>().GetAll());

            foreach (var item in writers)
            {
                if (!string.IsNullOrEmpty(item.PhotoUrl))
                {
                    item.PhotoUrl = _cloudinary.Api.UrlImgUp.Secure().Transform(new Transformation().Height(500).Crop("scale")).BuildUrl(item.PhotoUrl + ".webp");
                }
            }
            return Ok(writers);
        }

        [Route("[action]")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var user = _mapper.Map<UserDto>(await _unitOfWork.GetRepository<User>().Get(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)));

            if (!string.IsNullOrEmpty(user.PhotoUrl))
            {
                user.PhotoUrl = _cloudinary.Api.UrlImgUp.Secure().Transform(new Transformation().Height(500).Crop("scale")).BuildUrl(user.PhotoUrl + ".webp");
            }

            return Ok(user);
        }

        [Route("[action]")]
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto userDto)
        {
            if (int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) != userDto.Id)
            {
                return Unauthorized();
            }

            var user = await _unitOfWork.GetRepository<User>().Get(userDto.Id);
            user.Surname = userDto.Surname;
            user.Name = userDto.Name;
            user.Background = userDto.Background;
            user.BirthDate = userDto.BirthDate;

            try
            {
                await _unitOfWork.GetRepository<User>().Update(user);
                await _unitOfWork.Save();

                return StatusCode(200);
            }
            catch (Exception ex)
            {
                Extensions.ReportError(ex);
                throw;
            }

            return StatusCode(500);


        }


    }
}