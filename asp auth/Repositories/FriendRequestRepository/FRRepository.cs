using asp_auth.Models;
using asp_auth.Models.Entities;
using lab2.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Repositories
{
    public class FRRepository : GenericRepository<FriendRequest>, IFRRepository
    {
        public FRRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<List<FriendRequest>> GetFriendRequestsBySenderId(int senderId)
        {
            return await _context.FriendRequests.Where(fr => fr.SenderId.Equals(senderId)).ToListAsync();
        }

        public async Task<List<FriendRequest>> GetFriendRequestsByReceiverId(int receiverId)
        {
            return await _context.FriendRequests.Where(fr => fr.ReceiverId.Equals(receiverId)).ToListAsync();
        }
    }
}
