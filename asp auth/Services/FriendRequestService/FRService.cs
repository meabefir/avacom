using asp_auth.Models.Entities;
using asp_auth.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Services
{
    public class FRService : IFRService
    {
        //private UserManager<User> _userManager;
        private readonly UserManager<User> _userManager;
        //private UserManager<User> _userManager;
        private readonly IRepositoryWrapper _repository;

        public FRService(UserManager<User> userManager, IRepositoryWrapper repository)
        {
            _userManager = userManager;
            _repository = repository;
        }

        public async  void DeleteFriendRequest(string username, int currentUserId)
        {
            var sender = await _repository.User.FindByUsernameAsync(username);
            var receiver = await _repository.User.GetByIdAsync(currentUserId);

            var fr = await _repository.FriendRequest.GetByIdAsync(sender.Id, receiver.Id);

            _repository.FriendRequest.Delete(fr);
        }
    }
}
