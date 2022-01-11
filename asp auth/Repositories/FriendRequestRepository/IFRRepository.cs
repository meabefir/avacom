using asp_auth.Models.Entities;
using lab2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Repositories
{
    public interface IFRRepository: IGenericRepository<FriendRequest>
    {
        Task<List<FriendRequest>> GetFriendRequestsBySenderId(int receiverId);
        Task<List<FriendRequest>> GetFriendRequestsByReceiverId(int receiverId);
    }
}
