using asp_auth.Models.DTOs;
using asp_auth.Models.Entities;
using asp_auth.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;

        public PostController(IRepositoryWrapper repo)
        {
            _repository = repo;
        }

        [HttpPost("create")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDTO dto_post)
        {
            Post new_post = new Post();
            new_post.UserId = dto_post.UserId;
            new_post.Title = dto_post.Title;
            new_post.Text = dto_post.Text;
            new_post.CreatedAt = DateTime.Now;

            if (User.Identity.Name != new_post.UserId.ToString())
                return BadRequest("attempted to create post with different used id");

            // User sender_user = _repository.User.fin
            Console.WriteLine("user " + User.Identity.Name + " made a post");

            _repository.Post.Create(new_post);

            await _repository.SaveAsync();

            return Ok(new_post);
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetPostsByUserId(int userId)
        {
            var posts = await _repository.Post.GetPostsByUserId(userId);

            return Ok(posts);
        }
    }
}
