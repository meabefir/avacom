using asp_auth.Models.DTOs;
using asp_auth.Models.Entities;
using asp_auth.Repositories;
using asp_auth.Services;
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
        private readonly IFRService _frService;

        public FriendRequestController(IRepositoryWrapper repo,
                                        IFRService frService)
        {
            _repository = repo;
            _frService = frService;
        }

        [HttpPost("send/{username}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> SendFriendRequest(string username)
        {
            var receiver = await _repository.User.FindByUsernameAsync(username);
            var sender = await _repository.User.GetByIdAsync(Int32.Parse(User.Identity.Name));

            if (receiver == null)
            {
                // nu exista user cu numele asta
                return Ok(new { message = "User not found." });
            }

            if (sender.UserName == username)
                return Ok(new { message = "Can't send friend request to yourself."});

            FriendRequest new_fr = new FriendRequest();
            new_fr.Sender = sender;
            new_fr.Receiver = receiver;
            new_fr.SenderId = Int32.Parse(User.Identity.Name);
            new_fr.ReceiverId = receiver.Id;
            new_fr.SentAt = DateTime.Now;

            // User sender_user = _repository.User.fin
            Console.WriteLine("user " + sender.UserName + " sent a friend request to user " + receiver.UserName);
            try
            {
                _repository.FriendRequest.Create(new_fr);
            }
            catch (Exception e)
            {
                return Ok(new { message = "Pending friend request with that user already exists." });
            }

            await _repository.SaveAsync();

            return Ok(new { message = "Friend request sent." });
        }

        [HttpDelete("delete/{username}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteFriendRequest(string username)
        {
            // _frService.DeleteFriendRequest(username, Int32.Parse(User.Identity.Name));
            var sender = await _repository.User.FindByUsernameAsync(username);
            var receiver = await _repository.User.GetByIdAsync(Int32.Parse(User.Identity.Name));

            var fr = await _repository.FriendRequest.GetByIdAsync(sender.Id, receiver.Id);

            if (fr == null)
            {
                return BadRequest(new { message = "Friend request not found." });
            }

            _repository.FriendRequest.Delete(fr);

            await _repository.SaveAsync();

            return Ok(new { message = "Friend request declined." });
        }

        [HttpGet("accept/{username}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AcceptFriendRequest(string username)
        {
            // _frService.DeleteFriendRequest(username, Int32.Parse(User.Identity.Name));
            var sender = await _repository.User.FindByUsernameAsync(username);
            var receiver = await _repository.User.GetByIdAsync(Int32.Parse(User.Identity.Name));

            var fr = await _repository.FriendRequest.GetByIdAsync(sender.Id, receiver.Id);

            if (fr == null)
            {
                return BadRequest(new { message = "Friend request not found." });
            }

            _repository.FriendRequest.Delete(fr);

            // add friend relation
            // verific daca exista deja relatia de prietenie
            var f = await _repository.Friend.GetByIdAsync(sender.Id, receiver.Id);

            if (f != null)
            {
                await _repository.SaveAsync();
                return Ok(new { message = "Already friends with this user." });
            }

            Friend new_f = new Friend();
            new_f.User1 = sender;
            new_f.User2 = receiver;
            new_f.User1Id = sender.Id;
            new_f.User2Id = receiver.Id;
            new_f.FriendsSince = DateTime.Now;
            _repository.Friend.Create(new_f);

            await _repository.SaveAsync();

            return Ok(new { message = "Friend request confirmed."});
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
