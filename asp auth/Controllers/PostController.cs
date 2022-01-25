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
            var sender = await _repository.User.GetByIdAsync(Int32.Parse(User.Identity.Name));

            Post new_post = new Post();
            new_post.UserId = Int32.Parse(User.Identity.Name);
            new_post.Title = dto_post.Title;
            new_post.Text = dto_post.Text;
            new_post.CreatedAt = DateTime.Now;
            new_post.User = sender;

            // User sender_user = _repository.User.fin
            Console.WriteLine("user " + User.Identity.Name + " made a post");

            _repository.Post.Create(new_post);

            await _repository.SaveAsync();

            var posting_user = await _repository.User.GetByIdAsync(Int32.Parse(User.Identity.Name));

            PostView pw = new PostView();
            pw.Username = posting_user.UserName;
            pw.Text = new_post.Text;
            pw.Title = new_post.Title;
            pw.CreatedAt = new_post.CreatedAt;

            return Ok(pw);
        }

        [HttpPost("comment")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Comment([FromBody] CommentDTO comm_dto)
        {
            var sender = await _repository.User.GetByIdAsync(Int32.Parse(User.Identity.Name));
            var post = await _repository.Post.GetByIdAsync(comm_dto.PostId);

            Comment new_comm = new Comment();
            new_comm.UserId = Int32.Parse(User.Identity.Name);
            new_comm.PostId = comm_dto.PostId;
            new_comm.Text = comm_dto.Text;
            new_comm.CreatedAt = DateTime.Now;
            new_comm.User = sender;
            new_comm.Post = post;

            // User sender_user = _repository.User.fin
            Console.WriteLine("user " + User.Identity.Name + "commented on post with id " + new_comm.PostId);

            _repository.Comment.Create(new_comm);

            await _repository.SaveAsync();

            var posting_user = await _repository.User.GetByIdAsync(Int32.Parse(User.Identity.Name));

            CommentView pw = new CommentView();
            pw.Username = posting_user.UserName;
            pw.Text = comm_dto.Text;

            return Ok(pw);
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetPostsByUserId(int userId)
        {
            var posts = await _repository.Post.GetPostsByUserId(userId);

            return Ok(posts);
        }

        [HttpDelete("{postId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var post = await _repository.Post.GetByIdAsync(postId);

            var comments = _repository.Comment.GetAll().ToList();
            _repository.Comment.DeleteRange(comments);

            var reactions = _repository.PostReaction.GetAll().ToList();
            _repository.PostReaction.DeleteRange(reactions);

            await _repository.SaveAsync();

            _repository.Post.Delete(post);

            await _repository.SaveAsync();

            return Ok(new { message = "Post deleted" });
        }

        [HttpGet("feed")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetFeed()
        {
            var posts = await _repository.Post.GetFeed(Int32.Parse(User.Identity.Name));

            return Ok(posts);
        }
    }
}
