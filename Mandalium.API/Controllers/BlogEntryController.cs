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

namespace Mandalium.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogEntryController : ControllerBase
    {
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


        #region  get methods


        [HttpGet(Name = "GetEntries")]
        public async Task<IActionResult> GetEntries([FromQuery]UserParams userParams)
        {
            var entries = await _repo.GetBlogEntries(userParams);

            var returndto = _mapper.Map<IEnumerable<BlogEntryListDto>>(entries);

            foreach (var item in returndto)
            {
                item.PhotoUrl = _cloudinary.Api.UrlImgUp.Secure().Transform(new Transformation().Height(250).Crop("scale")).BuildUrl(item.PhotoUrl.Split("/").Last().Split(".").First() + ".webp");
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
                blogEntryDto.PhotoUrl = _cloudinary.Api.UrlImgUp.Secure().Transform(new Transformation().Height(500).Crop("scale")).BuildUrl(blogEntryDto.PhotoUrl.Split("/").Last().Split(".").First() + ".webp");
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
                item.PhotoUrl = _cloudinary.Api.UrlImgUp.Secure().Transform(new Transformation().Height(250).Crop("scale")).BuildUrl(item.PhotoUrl.Split("/").Last().Split(".").First() + ".webp");
            }
            foreach (var item in blogEntries)
            {
                item.PhotoUrl = _cloudinary.Api.UrlImgUp.Secure().Transform(new Transformation().Height(250).Crop("scale")).BuildUrl(item.PhotoUrl.Split("/").Last().Split(".").First() + ".webp");
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
        [HttpGet]
        public async Task<IActionResult> GetTopicAndWriter()
        {
            var topics = _mapper.Map<IEnumerable<TopicDto>>(await _repo.GetTopics());
            var writers = _mapper.Map<IEnumerable<WriterDto>>(await _repo.GetWriters());

            return Ok(new
            {
                topics,
                writers
            });
        }

        [Route("[action]")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveTopic(TopicDto topicDto)
        {
           
            var topic =  _mapper.Map<Topic>(topicDto);
            var number = await _repo.SaveTopic(topic);
            if ( number != 0)
            {
                 return StatusCode(200);
            }    
            return StatusCode(400);
        }

        #endregion



        #region  save methods

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveBlogEntry(BlogEntryForCreationDto blogEntryForCreationDto)
        {
            var blogEntry = _mapper.Map<BlogEntry>(blogEntryForCreationDto);
            await _repo.SaveBlogEntry(blogEntry);

            return RedirectToRoute("GetEntries");
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
