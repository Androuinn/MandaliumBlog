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
using Microsoft.Extensions.Caching.Memory;

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
        private readonly IMemoryCache _memoryCache;
        public BlogEntryController(IBlogRepository<BlogEntry> repo, IMapper mapper, IOptions<CloudinarySettings> _cloudinaryConfig, IMemoryCache memoryCache)
        {
            this._memoryCache = memoryCache;
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
        public async Task<IActionResult> GetEntries([FromQuery] UserParams userParams)
        {
            // string cachename = "entries" + userParams.WriterEntry.ToString() + userParams.PageNumber.ToString();

            // var entries = await _memoryCache.GetOrCreateAsync(cachename, entry =>
            // {
            //     entry.SlidingExpiration = TimeSpan.FromMinutes(1);
            //     entry.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5);
            //     return _repo.GetBlogEntries(userParams);

            // });

            try
            {
                var entries = await _repo.GetBlogEntries(userParams);

                var returndto = _mapper.Map<IEnumerable<BlogEntryListDto>>(entries);

                foreach (var item in returndto)
                {
                    if (!string.IsNullOrEmpty(item.PhotoUrl))
                    {
                         item.PhotoUrl = _cloudinary.Api.UrlImgUp.Secure().Transform(new Transformation().Height(250).Crop("scale")).BuildUrl(item.PhotoUrl + ".webp");
                    }
                }

                Response.AddPagination(entries.CurrentPage, entries.PageSize, entries.TotalCount, entries.TotalPages);
                
                return Ok(returndto);
            }
            catch (System.Exception ex)
            {

                Extensions.ReportError(ex);
            }

            return StatusCode(500);

        }


        [HttpGet("{id}", Name = "GetEntry")]
        public async Task<IActionResult> GetEntry(int id, [FromQuery] UserParams userParams)
        {
            // string cachename = "entry" + id.ToString() + userParams.PageNumber.ToString();
            // if(userParams.EntryAlreadyPicked == true) _memoryCache.Remove(cachename);
            // var blogEntry = await _memoryCache.GetOrCreateAsync(cachename, entry =>
            // {
            //     entry.SlidingExpiration = TimeSpan.FromMinutes(1);
            //     entry.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5);
            //     return _repo.GetBlogEntry(id, userParams);

            // });


            try
            {
                var blogEntry = await _repo.GetBlogEntry(id, userParams);

                if (blogEntry.isDeleted == true)
                {
                    return BadRequest("Entry Bulunamadı");
                }

                var blogEntryDto = _mapper.Map<BlogEntryDto>(blogEntry);

                if (userParams.EntryAlreadyPicked == false && !string.IsNullOrEmpty(blogEntryDto.PhotoUrl))
                {
                    blogEntryDto.PhotoUrl = _cloudinary.Api.UrlImgUp.Secure().Transform(new Transformation().Height(500).Crop("scale")).BuildUrl(blogEntryDto.PhotoUrl + ".webp");
                }


                Response.AddPagination(blogEntry.Comments.CurrentPage, blogEntry.Comments.PageSize, blogEntry.Comments.TotalCount, blogEntry.Comments.TotalPages);
                 var a = new MethodCallResponse<BlogEntryDto>(){entity= blogEntryDto, Message=null, StatusCode = ReturnCodes.OK};
                return Ok(a);
            }
            catch (System.Exception ex)
            {

                Extensions.ReportError(ex);
            }
            return StatusCode(500);

        }



        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetComments(int id, [FromQuery] UserParams userParams)
        {
            try
            {
                var comments = await _repo.GetComments(id, userParams);
                Response.AddPagination(comments.CurrentPage, comments.PageSize, comments.TotalCount, comments.TotalPages);
                var commentsDto = _mapper.Map<IEnumerable<CommentDto>>(comments);
                return Ok(commentsDto);
            }
            catch (System.Exception ex)
            {

                Extensions.ReportError(ex);
            }
            return StatusCode(500);


        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetMostRead()
        {
            // string cachename = "personalEntries";

            // var personalEntries = await _memoryCache.GetOrCreateAsync(cachename, entry =>
            // {
            //     entry.SlidingExpiration = TimeSpan.FromMinutes(30);
            //     entry.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(60);
            //     return _repo.GetMostRead(true);

            // });

            // cachename = "blogEntries";

            // var blogEntries = await _memoryCache.GetOrCreateAsync(cachename, entry =>
            // {
            //     entry.SlidingExpiration = TimeSpan.FromMinutes(30);
            //     entry.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(60);
            //     return _repo.GetMostRead(false);

            // });

            try
            {
                var personalEntries = await _repo.GetMostRead(true);
                var blogEntries = await _repo.GetMostRead(false);

                foreach (var item in personalEntries)
                {
                    if (!item.PhotoUrl.EndsWith("webp") && !string.IsNullOrEmpty(item.PhotoUrl))
                    {
                        item.PhotoUrl = _cloudinary.Api.UrlImgUp.Secure().Transform(new Transformation().Height(250).Crop("scale")).BuildUrl(item.PhotoUrl + ".webp");
                    }
                }
                foreach (var item in blogEntries)
                {
                    if (!item.PhotoUrl.EndsWith("webp") && !string.IsNullOrEmpty(item.PhotoUrl))
                    {
                        item.PhotoUrl = _cloudinary.Api.UrlImgUp.Secure().Transform(new Transformation().Height(250).Crop("scale")).BuildUrl(item.PhotoUrl + ".webp");
                    }
                }

                var mostReadPersonalEntriesDto = _mapper.Map<IEnumerable<BlogEntryListDto>>(personalEntries);
                var mostReadEntriesDto = _mapper.Map<IEnumerable<BlogEntryListDto>>(blogEntries);

                return Ok(new
                {
                    mostReadPersonalEntriesDto,
                    mostReadEntriesDto
                });
            }
            catch (System.Exception ex)
            {

                Extensions.ReportError(ex);
            }
            return StatusCode(500);

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

            try
            {
                var topics = _mapper.Map<IEnumerable<TopicDto>>(await _repo.GetTopics());
                return Ok(topics);
            }
            catch (System.Exception ex)
            {

                Extensions.ReportError(ex);
            }
            return StatusCode(500);

        }


        #endregion



        #region  save methods

        [Route("[action]")]
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> DeleteBlogEntry([FromBody] int id)
        {

            if (User.FindFirst(ClaimTypes.Role).Value != "1")
            {
                return Unauthorized();
            }

            try
            {
                if (await _repo.DeleteBlogEntry(id) != 0)
                {
                    return StatusCode(200);
                }
            }
            catch (System.Exception ex)
            {

                Extensions.ReportError(ex);
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

            try
            {
                var topic = _mapper.Map<Topic>(topicDto);
                var number = await _repo.SaveTopic(topic);
                if (number != 0)
                {
                    return StatusCode(200);
                }
            }
            catch (System.Exception ex)
            {

                Extensions.ReportError(ex);
            }


            return StatusCode(400);
        }




        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveBlogEntry(BlogEntryForCreationDto blogEntryForCreationDto)
        {
            if (blogEntryForCreationDto.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) || User.FindFirst(ClaimTypes.Role).Value != "1")
            {
                return Unauthorized();
            }

            try
            {
                var blogEntry = _mapper.Map<BlogEntry>(blogEntryForCreationDto);
                await _repo.SaveBlogEntry(blogEntry);
                return StatusCode(200);
            }
            catch (System.Exception ex)
            {

                Extensions.ReportError(ex);
            }

            return StatusCode(500);


        }


        [Route("[action]")]
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateBlogEntry(BlogEntryForCreationDto blogEntryForCreationDto)
        {
            if (blogEntryForCreationDto.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) || User.FindFirst(ClaimTypes.Role).Value != "1")
            {
                return Unauthorized();
            }

            try
            {
                var blogEntry = _mapper.Map<BlogEntry>(blogEntryForCreationDto);
                await _repo.UpdateBlogEntry(blogEntry);

                return StatusCode(200);
            }
            catch (System.Exception ex)
            {

                Extensions.ReportError(ex);
            }
            return StatusCode(500);


        }



        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> WriteComment(CommentDtoForCreation commentDtoForCreation)
        {

            try
            {
                var comment = _mapper.Map<Comment>(commentDtoForCreation);
                if (await _repo.SaveComment(comment) >= 0)
                {

                    return Ok(_mapper.Map<CommentDto>(comment));
                }
                return BadRequest();
                // return RedirectToRoute("GetEntry",comment.BlogEntryId);
            }
            catch (System.Exception ex)
            {

                Extensions.ReportError(ex);
            }

            return StatusCode(500);

        }

        #endregion





    }
}
