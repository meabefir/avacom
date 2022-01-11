using asp_auth.Models.Constants;
using asp_auth.Models.DTOs;
using asp_auth.Models.Entities;
using asp_auth.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;

        public AccountController(
            UserManager<User> userManager,
            IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO dto)
        {
            var exists = await _userManager.FindByNameAsync(dto.Username);
            
            if (exists != null)
            {
                return BadRequest(new { message = "Username not available." });
            }

            var result = await _userService.RegisterUserAsync(dto);
            if (result == "true")
            {
                return Ok(new { message = "User registered successfuly." });
            }
            else if (result == "false")
            {
                return BadRequest(new { message = "Unknown error." });
            }
            else
            {
                return BadRequest(new { message = result });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO dto)
        {
            var token = await _userService.LoginUser(dto);

            if (token.Equals("Wrong password."))
            {
                return Unauthorized(new { message = token });
            }
            else if (token.Equals("User doesnt exist."))
            {
                return Unauthorized(new { message = token });
            }
            else
            {
                return Ok(new { token });
            }
        }
    }
}
