using asp_auth.Models;
using asp_auth.Models.DTOs;
using asp_auth.Models.Entities;
using lab2.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Repositories
{
    public class FriendRepository : GenericRepository<Friend>, IFriendRepository
    {
        public FriendRepository(AppDbContext context) : base(context)
        {
            
        }

        public async Task<Friend> GetByIdAsync(int senderId, int receiverId)
        {
            return await _context.Friends
                .Where(f => ((f.User1Id.Equals(senderId) && f.User2Id.Equals(receiverId)) ||
                              (f.User1Id.Equals(receiverId) && f.User2Id.Equals(senderId))))
                .FirstOrDefaultAsync();
        }

        public async Task<List<FriendView>> GetFriends(int userId)
        {
            return await _context.Friends.Where(f => (f.User1Id == userId || f.User2Id == userId) && f.User1Id != f.User2Id).Select(f => 
                new FriendView
                {
                    Username = f.User1Id == userId ? f.User2.UserName : f.User1.UserName,
                    FriendsSince = f.FriendsSince
                }
            )
                .OrderBy(fw => fw.FriendsSince)
                .ToListAsync();
        }
    }
}