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
    public class PostReactionRepository : GenericRepository<PostReaction>, IPostReactionRepository
    {
        public PostReactionRepository(AppDbContext context) : base(context)
        {

        }

        async public Task<string> DeleteFrom(int userId, int postId)
        {
            var to_delete = await _context.PostReactions.Where(pr => (pr.UserId == userId && pr.PostId == postId)).ToListAsync();
            _context.PostReactions.RemoveRange(to_delete);
            return "ok";
        }

        public async Task<List<PostReaction>> GetPostReactionsByPostId(int postId)
        {
            return await _context.PostReactions.Where(pr => pr.PostId.Equals(postId)).ToListAsync();
        }
    }
}
