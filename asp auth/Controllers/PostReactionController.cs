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

        [HttpGet("{postId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetPostReactionsWithPostId(int postId)
        {
            var posts = await _repository.PostReaction.GetPostReactionsByPostId(postId);

            return Ok(posts);
        }
    }
}
