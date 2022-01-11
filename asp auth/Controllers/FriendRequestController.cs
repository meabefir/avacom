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
    public class FriendRequestController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;

        public FriendRequestController(IRepositoryWrapper repo)
        {
            _repository = repo;
        }

        [HttpPost("create")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> SendFriendRequest([FromBody] FriendRequestDTO dto_fr)
        {
            FriendRequest new_fr = new FriendRequest();
            new_fr.SenderId = dto_fr.SenderId;
            new_fr.ReceiverId = dto_fr.ReceiverId;
            new_fr.SentAt = DateTime.Now;

            if (User.Identity.Name != new_fr.SenderId.ToString())
                return BadRequest("attempted to send a FR with different sender id");

            // User sender_user = _repository.User.fin
            Console.WriteLine("user " + User.Identity.Name + " sent a friend request to user " + dto_fr.ReceiverId);

            _repository.FriendRequest.Create(new_fr);

            await _repository.SaveAsync();

            return Ok(new_fr);
        }

        [HttpGet("{receiverId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetFriendRequestsByReceiverId(int receiverId)
        {
            if (User.Identity.Name != receiverId.ToString())
                return BadRequest("attempted to fetch friends requests of another user");

            var posts = await _repository.FriendRequest.GetFriendRequestsByReceiverId(receiverId);

            return Ok(posts);
        }
    }
}
