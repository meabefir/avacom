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

        public async Task<List<PostReaction>> GetPostReactionsByPostId(int post_id)
        {
            return await _context.PostReactions.Where(pr => pr.PostId.Equals(post_id)).ToListAsync();
        }
    }
}
