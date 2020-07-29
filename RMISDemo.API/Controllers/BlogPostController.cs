using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMISDemo.API.Models;
using RMISDemo.API.Models.Dto;
using RMISDemo.API.Models.Repository;

namespace RMISDemo.API.Controllers
{
    [Authorize]
    [Route("api/blogs")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly IDataRepository<BlogPost, BlogPostDto> _repo;

        public BlogPostController(IDataRepository<BlogPost, BlogPostDto> dataRepository)
        {
            _repo = dataRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            // Fetch all blog posts
            var posts = _repo.GetAll();
            if (posts == null)
            {
                return NotFound("No Posts not found.");
            }
            return Ok(posts);
        }

        [HttpGet("{keyword}")]
        public IActionResult GetAllForKeyword(string keyword)
        {
            // Fetch all blog posts with the keyword in the title
            var posts = _repo.GetAllForKeyword(keyword);
            if (posts == null)
            {
                return NotFound("No Posts not found with the provided keyword.");
            }
            return Ok(posts);
        }
    }
}
