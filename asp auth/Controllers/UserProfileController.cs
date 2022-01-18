using asp_auth.Models.DTOs;
using asp_auth.Repositories;
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
    public class UserProfileController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;

        public UserProfileController(IRepositoryWrapper repo)
        {
            _repository = repo;
        }

        [HttpGet("myProfile")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetMyProfile()
        {
            var up_view = await _repository.UserProfile.GetByPk(Int32.Parse(User.Identity.Name));

            return Ok(up_view);
        }

        [HttpPut("myProfile")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UserProfileDTO up_dto)
        {
            var up = await _repository.UserProfile.GetByPkRaw(Int32.Parse(User.Identity.Name));

            up.Bio = up_dto.Bio;
            up.Nickname = up_dto.Bio;
            up.Age = up_dto.Age;
            _repository.UserProfile.Update(up);
            await _repository.SaveAsync();

            return Ok(new UserProfileView
            {
                Bio = up.Bio,
                Nickname = up.Nickname,
                Age = up.Age
            });
        }

        [HttpGet("profile/{username}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserProfile(string username)
        {
            var up_view = await _repository.UserProfile.GetUserProfile(username);
            
            if (up_view == null)
            {
                return BadRequest(new { message = "User not found." });
            }

            return Ok(up_view);
        }
    }
}
