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

        [HttpPost("send/{username}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> SendFriendRequest(string username)
        {
            var receiver = await _repository.User.FindByUsernameAsync(username);
            var sender = await _repository.User.GetByIdAsync(Int32.Parse(User.Identity.Name));

            FriendRequest new_fr = new FriendRequest();
            new_fr.SenderId = Int32.Parse(User.Identity.Name);
            new_fr.ReceiverId = receiver.Id;
            new_fr.SentAt = DateTime.Now;

            // User sender_user = _repository.User.fin
            Console.WriteLine("user " + sender.UserName + " sent a friend request to user " + receiver.UserName);

            _repository.FriendRequest.Create(new_fr);

            await _repository.SaveAsync();

            return Ok();
        }

        [HttpDelete("delete/{username}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteFriendRequest(string username)
        {
            var sender = await _repository.User.FindByUsernameAsync(username);
            var receiver = await _repository.User.GetByIdAsync(Int32.Parse(User.Identity.Name));

            var fr = await _repository.FriendRequest.GetByIdAsync(sender.Id, receiver.Id);

            _repository.FriendRequest.Delete(fr);

            await _repository.SaveAsync();

            return Ok();
        }

        [HttpGet("received")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetFriendRequestsByReceiverId()
        {

            var user = await _repository.User.GetByIdAsync(Int32.Parse(User.Identity.Name));

            var posts = await _repository.FriendRequest.GetFriendRequestsByReceiverUsername(user.UserName);

            return Ok(posts);
        }
    }
}
