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
    public class FriendController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;

        public FriendController(IRepositoryWrapper repo)
        {
            _repository = repo;
        }

        [HttpGet]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetFriends()
        {
            var friends = await _repository.Friend.GetFriends(Int32.Parse(User.Identity.Name));

            return Ok(friends);
        }
    }
}
