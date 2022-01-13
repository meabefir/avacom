using asp_auth.Models.Entities;
using asp_auth.Models.Views;
using lab2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Repositories
{
    public interface IFRRepository: IGenericRepository<FriendRequest>
    {
        Task<List<FriendRequestView>> GetFriendRequestsBySenderUsername(string username);
        Task<List<FriendRequestView>> GetFriendRequestsByReceiverUsername(string username);

        Task<FriendRequest> GetByIdAsync(int senderId, int receiverId);
    }
}
