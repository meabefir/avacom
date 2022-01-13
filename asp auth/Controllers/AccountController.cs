using asp_auth.Models.Constants;
using asp_auth.Models.DTOs;
using asp_auth.Models.Entities;
using asp_auth.Repositories;
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
        private readonly IRepositoryWrapper _repository;

        public AccountController(
            UserManager<User> userManager,
            IUserService userService,
            IRepositoryWrapper repo)
        {
            _userManager = userManager;
            _userService = userService;
            _repository = repo;
        }

        [HttpGet("confirmEmail/{username}/{token}")]
        [AllowAnonymous]
        public void ConfirmEmail(string username, string token)
        {
            _userService.ConfirmEmail(username, token);
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
                var user = await _userManager.FindByNameAsync(dto.Username);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                if (!GlobalConstants.sendEmailConfirmation)
                    await _userManager.ConfirmEmailAsync(user, token);
                if (GlobalConstants.sendEmailConfirmation)
                {
                    // email confirmation
                    // asta nu a mers idk why
                    //var confirmationLink = Url.Action("confirmEmail", "account",
                    //                            new { userId = user.Id, token = token });

                    // trb sa am grija cum trimit tokenu asta in url (l-am facut jwt)
                    var confirmationLink = "https://localhost:5001/api/account/confirmEmail/" + user.UserName + "/" + _userService.tokenToJWT(token);

                    // send this to user email for confirmation (right now only logging it in console)
                    Console.WriteLine("\nconfirmation link " + confirmationLink);
                }

                return Ok(new { message = "User registered successfuly. Please confirm your email" });
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
            else if (token.Equals("Please confirm your email."))
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
