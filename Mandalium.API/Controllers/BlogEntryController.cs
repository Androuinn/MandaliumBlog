﻿using System;
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
    public class BlogEntryController : ControllerBase
    {

        #region field and constructor
        private readonly IBlogRepository<BlogEntry> _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> cloudinaryConfig;
        private Cloudinary _cloudinary;
        public BlogEntryController(IBlogRepository<BlogEntry> repo, IMapper mapper, IOptions<CloudinarySettings> _cloudinaryConfig)
        {
            this.cloudinaryConfig = _cloudinaryConfig;
            this._mapper = mapper;
            this._repo = repo;

            Account acc = new Account(
                 _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }
        #endregion


        #region  get methods


        [HttpGet(Name = "GetEntries")]
        public async Task<IActionResult> GetEntries([FromQuery]UserParams userParams)
        {
            var entries = await _repo.GetBlogEntries(userParams);

            var returndto = _mapper.Map<IEnumerable<BlogEntryListDto>>(entries);

            foreach (var item in returndto)
            {
                item.PhotoUrl = _cloudinary.Api.UrlImgUp.Secure().Transform(new Transformation().Height(250).Crop("scale")).BuildUrl(item.PhotoUrl + ".webp");
            }

            Response.AddPagination(entries.CurrentPage, entries.PageSize, entries.TotalCount, entries.TotalPages);

            return Ok(returndto);
        }


        [HttpGet("{id}", Name = "GetEntry")]
        public async Task<IActionResult> GetEntry(int id, [FromQuery] UserParams userParams)
        {

            var blogEntry = await _repo.GetBlogEntry(id, userParams);

            var blogEntryDto = _mapper.Map<BlogEntryDto>(blogEntry);

            if (userParams.EntryAlreadyPicked == false)
            {
                blogEntryDto.PhotoUrl = _cloudinary.Api.UrlImgUp.Secure().Transform(new Transformation().Height(500).Crop("scale")).BuildUrl(blogEntryDto.PhotoUrl + ".webp");
            }


            Response.AddPagination(blogEntry.Comments.CurrentPage, blogEntry.Comments.PageSize, blogEntry.Comments.TotalCount, blogEntry.Comments.TotalPages);

            return Ok(blogEntryDto);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetMostRead()
        {
            var personalEntries = await _repo.GetMostRead(true);
            var blogEntries = await _repo.GetMostRead(false);

            foreach (var item in personalEntries)
            {
                item.PhotoUrl = _cloudinary.Api.UrlImgUp.Secure().Transform(new Transformation().Height(250).Crop("scale")).BuildUrl(item.PhotoUrl + ".webp");
            }
            foreach (var item in blogEntries)
            {
                item.PhotoUrl = _cloudinary.Api.UrlImgUp.Secure().Transform(new Transformation().Height(250).Crop("scale")).BuildUrl(item.PhotoUrl + ".webp");
            }

            var mostReadPersonalEntriesDto = _mapper.Map<IEnumerable<BlogEntryListDto>>(personalEntries);
            var mostReadEntriesDto = _mapper.Map<IEnumerable<BlogEntryListDto>>(blogEntries);

            return Ok(new
            {
                mostReadPersonalEntriesDto,
                mostReadEntriesDto
            });
        }

        [Route("[action]")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetTopics()
        {
            if (User.FindFirst(ClaimTypes.Role).Value != "1")
            {
                return Unauthorized();
            }

            var topics = _mapper.Map<IEnumerable<TopicDto>>(await _repo.GetTopics());
            

            return Ok(topics);
        }

       
        #endregion



        #region  save methods
        
        [Route("[action]")]
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> DeleteBlogEntry([FromBody]int id)
        {
            if (User.FindFirst(ClaimTypes.Role).Value != "1" )
            {
                return Unauthorized();
            }

            if (await _repo.DeleteBlogEntry(id) != 0)
            {
                return StatusCode(200);
            }
            return BadRequest();
        }


        [Route("[action]")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveTopic(TopicDto topicDto)
        {
            if (User.FindFirst(ClaimTypes.Role).Value != "1")
            {
                return Unauthorized();
            }

            var topic = _mapper.Map<Topic>(topicDto);
            var number = await _repo.SaveTopic(topic);
            if (number != 0)
            {
                return StatusCode(200);
            }
            return StatusCode(400);
        }




        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveBlogEntry(BlogEntryForCreationDto blogEntryForCreationDto)
        {
            if (blogEntryForCreationDto.WriterId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) || User.FindFirst(ClaimTypes.Role).Value != "1")
            {
                return Unauthorized();
            }

            var blogEntry = _mapper.Map<BlogEntry>(blogEntryForCreationDto);
            await _repo.SaveBlogEntry(blogEntry);

            return StatusCode(200);
        }


        [Route("[action]")]
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateBlogEntry(BlogEntryForCreationDto blogEntryForCreationDto)
        {
            if (blogEntryForCreationDto.WriterId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) || User.FindFirst(ClaimTypes.Role).Value != "1")
            {
                return Unauthorized();
            }


            var blogEntry = _mapper.Map<BlogEntry>(blogEntryForCreationDto);
            await _repo.UpdateBlogEntry(blogEntry);

            return StatusCode(200);
        }



        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> WriteComment(CommentDtoForCreation commentDtoForCreation)
        {
            var comment = _mapper.Map<Comment>(commentDtoForCreation);
            await _repo.SaveComment(comment);

            // return RedirectToRoute("GetEntry",comment.BlogEntryId);
            return NoContent();
        }

        #endregion





    }
}
