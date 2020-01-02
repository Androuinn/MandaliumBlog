using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Mandalium.API.Data;
using Mandalium.API.Dtos;
using Mandalium.API.Helpers;
using Mandalium.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mandalium.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogEntryController : ControllerBase
    {
        private readonly IBlogRepository<BlogEntry> _repo;
        private readonly IMapper _mapper;
        public BlogEntryController(IBlogRepository<BlogEntry> repo, IMapper mapper)
        {
            this._mapper = mapper;
            this._repo = repo;
        }

        [HttpGet(Name = "GetEntries")]
        public async Task<IActionResult> GetEntries([FromQuery]UserParams userParams)
        {
            var entries = await _repo.GetBlogEntries(userParams);

            var returndto = _mapper.Map<IEnumerable<BlogEntryListDto>>(entries);

            Response.AddPagination(entries.CurrentPage, entries.PageSize, entries.TotalCount, entries.TotalPages);

            return Ok(returndto);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetMostRead()
        {
            var personalEntries = await _repo.GetMostRead(true);
            var blogEntries = await _repo.GetMostRead(false);

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

        [HttpGet("{id}", Name = "GetEntry")]
        public async Task<IActionResult> GetEntry(int id)
        {
            var blogEntry = await _repo.GetBlogEntry(id);
            var blogEntryDto = _mapper.Map<BlogEntryDto>(blogEntry);

            return Ok(blogEntryDto);
        }


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


    }
}
