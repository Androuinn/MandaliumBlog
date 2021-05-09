using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Mandalium.Core.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;
using Mandalium.Core.Interfaces;
using Mandalium.Core.Models;
using Mandalium.Core.Helpers.Pagination;
using Mandalium.Core.Dto;
using Mandalium.Resources;
using Mandalium.API.Models;

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
        private readonly IUnitOfWork _unitOfWork;
        public BlogEntryController(IBlogRepository<BlogEntry> repo, IUnitOfWork unitOfWork, IMapper mapper, IOptions<CloudinarySettings> _cloudinaryConfig, IMemoryCache memoryCache)
        {
            this._memoryCache = memoryCache;
            this.cloudinaryConfig = _cloudinaryConfig;
            this._mapper = mapper;
            this._repo = repo;
            _unitOfWork = unitOfWork;

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
            try
            {
                var entries = await _repo.GetBlogEntries(userParams);

                ConvertPhotoUrl(entries, 250);

                Response.AddPagination(entries.CurrentPage, entries.PageSize, entries.TotalCount, entries.TotalPages);

                return Ok(_mapper.Map<IEnumerable<BlogEntryListDto>>(entries));
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
            try
            {
                var blogEntry = await _repo.GetBlogEntry(id);

                if (blogEntry.IsDeleted)
                    return BadRequest("Entry Bulunamadı");

                var blogEntryDto = _mapper.Map<BlogEntryDto>(blogEntry);

                ConvertPhotoUrl(new List<BlogEntryDto>() { blogEntryDto }, 500);

                var a = new MethodCallResponse<BlogEntryDto>() { entity = blogEntryDto, Message = null, StatusCode = ReturnCodes.OK };
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
                ConvertPhotoUrl<CommentDto>(commentsDto, 250);

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
            var blogEntries = await _memoryCache.GetOrCreateAsync("mostReadEntries", entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(5);
                entry.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30);
                return _repo.GetMostRead();
            });

            try
            {
                ConvertPhotoUrl(blogEntries, 250);

                return Ok(_mapper.Map<IEnumerable<BlogEntryListDto>>(blogEntries));
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
                return Unauthorized();

            try
            {
                var topics = _mapper.Map<IEnumerable<TopicDto>>(await _unitOfWork.GetRepository<Topic>().GetAll());
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
                return Unauthorized();

            BlogEntry entry = await _unitOfWork.GetRepository<BlogEntry>().Get(id);
            if (entry == null)
                return BadRequest();

            try
            {
                entry.IsDeleted = true;
                await _unitOfWork.GetRepository<BlogEntry>().Update(entry);
                await _unitOfWork.Save();
                return StatusCode(200);
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
                return Unauthorized();

            try
            {
                var topic = _mapper.Map<Topic>(topicDto);
                await _unitOfWork.GetRepository<Topic>().Save(topic);
                await _unitOfWork.Save();
                return StatusCode(200);
            }
            catch (System.Exception ex)
            {

                Extensions.ReportError(ex);
            }


            return StatusCode(400);
        }




        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveBlogEntry(BlogCreateRequest request)
        {
            if (request.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) || User.FindFirst(ClaimTypes.Role).Value != "1")
                return Unauthorized();

            User user = await _unitOfWork.GetRepository<User>().Get(request.UserId);
            if (user == null)
                return Unauthorized();

            try
            {
                BlogEntry blog = new BlogEntry()
                {
                    WriterEntry = request.WriterEntry,
                    Headline = request.Headline,
                    PhotoUrl = request.PhotoUrl,
                    InnerTextHtml = request.innerTextHtml,
                    SubHeadline = request.SubHeadline,
                    Topic = await _unitOfWork.GetRepository<Topic>().Get(request.TopicId),
                    User = user
                };

                await _unitOfWork.GetRepository<BlogEntry>().Save(blog);
                await _unitOfWork.Save();
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
        public async Task<IActionResult> UpdateBlogEntry(BlogCreateRequest request)
        {
            if (request.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) || User.FindFirst(ClaimTypes.Role).Value != "1")
                return Unauthorized();

            User user = await _unitOfWork.GetRepository<User>().Get(request.UserId);
            if (user == null)
                return Unauthorized();

            try
            {
                BlogEntry blog = new BlogEntry()
                {
                    WriterEntry = request.WriterEntry,
                    Headline = request.Headline,
                    PhotoUrl = request.PhotoUrl,
                    InnerTextHtml = request.innerTextHtml,
                    SubHeadline = request.SubHeadline,
                    Topic = await _unitOfWork.GetRepository<Topic>().Get(request.TopicId),
                    User = user,
                    Id = request.Id
                };

                await _unitOfWork.GetRepository<BlogEntry>().Update(blog);
                await _unitOfWork.Save();
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
        public async Task<IActionResult> WriteComment(CommentCreateRequest request)
        {
            try
            {
                Comment comment = new Comment
                {
                    BlogEntry = await _unitOfWork.GetRepository<BlogEntry>().Get(request.BlogEntryId),
                    CommenterName = request.CommenterName,
                    Email = request.Email,
                    User = request.userId != null ? await _unitOfWork.GetRepository<User>().Get((int)request.userId) : null,
                    CommentString = request.CommentString,
                    CreatedDate = DateTime.Now
                };

                await _unitOfWork.GetRepository<Comment>().Save(comment);
                await _unitOfWork.Save();
                return Ok(comment);
            }
            catch (System.Exception ex)
            {
                Extensions.ReportError(ex);
            }

            return StatusCode(500);
        }

        #endregion

        private void ConvertPhotoUrl<TEntity>(IEnumerable<TEntity> list, int height = 250)
        {
            foreach (var listItem in list)
            {
                SetPhotoUrl(listItem, height);
            }
        }

        private void SetPhotoUrl<TEntity>(TEntity entity, int height)
        {
            var typePhotoUrl = entity.GetType().GetProperty("PhotoUrl").GetValue(entity) as string;
            if (typePhotoUrl != null && !typePhotoUrl.EndsWith("webp") && !string.IsNullOrEmpty(typePhotoUrl))
                entity.GetType().GetProperty("PhotoUrl").SetValue(entity, SetCloudinaryImageForPhotoUrl(typePhotoUrl, height));
        }
        private string SetCloudinaryImageForPhotoUrl(string url, int height)
        {
            return _cloudinary.Api.UrlImgUp.Secure().Transform(new Transformation().Height(height).Crop("scale")).BuildUrl(url + ".webp");
        }
    }
}
