using asp_auth.Models.DTOs;
using asp_auth.Models.Entities;
using lab2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Repositories
{
    public interface IFriendRepository : IGenericRepository<Friend>
    {
        Task<Friend> GetByIdAsync(int senderId, int receiverId);

        Task<List<FriendView>> GetFriends(int userId);
    }
}
