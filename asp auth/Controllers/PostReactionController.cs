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
    public class PostReactionController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;

        public PostReactionController(IRepositoryWrapper repo)
        {
            _repository = repo;
        }

        [HttpPost("create")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreatePostReaction([FromBody] CreatePostReactionDTO dto_post_reaction)
        {
            PostReaction new_post_reaction = new PostReaction();
            new_post_reaction.UserId = dto_post_reaction.UserId;
            new_post_reaction.PostId = dto_post_reaction.PostId;
            new_post_reaction.ReactionType = dto_post_reaction.ReactionType;

            if (User.Identity.Name != new_post_reaction.UserId.ToString())
                return BadRequest("attempted to create post reaction with different used id");

            // User sender_user = _repository.User.fin
            Console.WriteLine("user " + User.Identity.Name + " reacted to post " + new_post_reaction.PostId);

            _repository.PostReaction.Create(new_post_reaction);

            await _repository.SaveAsync();

            return Ok(new_post_reaction);
        }

        [HttpGet("{postId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetPostReactionsWithPostId(int postId)
        {
            var posts = await _repository.PostReaction.GetPostReactionsByPostId(postId);

            return Ok(posts);
        }
    }
}
