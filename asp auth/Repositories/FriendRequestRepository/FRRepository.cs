using asp_auth.Models;
using asp_auth.Models.Entities;
using asp_auth.Models.Views;
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

        public async Task<List<FriendRequestView>> GetFriendRequestsBySenderUsername(string username)
        {
            return await _context.FriendRequests
                .Where(fr => fr.Sender.UserName.Equals(username))
                .Select(fr => new FriendRequestView
                {
                    Username = fr.Receiver.UserName,
                    SentAt = fr.SentAt
                })
                .ToListAsync();
        }

        public async Task<List<FriendRequestView>> GetFriendRequestsByReceiverUsername(string username)
        {
            return await _context.FriendRequests
                .Where(fr => fr.Receiver.UserName.Equals(username))
                .Select(fr => new FriendRequestView
                {
                    Username = fr.Sender.UserName,
                    SentAt = fr.SentAt
                })
                .OrderBy(fr => fr.SentAt)
                .ToListAsync();
        }

        public async Task<FriendRequest> GetByIdAsync(int senderId, int receiverId)
        {
            return await _context.FriendRequests
                .Where(fr => ((fr.SenderId.Equals(senderId) && fr.ReceiverId.Equals(receiverId)) ||
                                (fr.SenderId.Equals(receiverId) && fr.ReceiverId.Equals(senderId))) )
                .FirstOrDefaultAsync();
        }
    }
}
