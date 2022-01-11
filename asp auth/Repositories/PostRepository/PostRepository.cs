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
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<List<Post>> GetPostsByUserId(int userId)
        {
            return await _context.Posts.Where(p => p.UserId.Equals(userId)).ToListAsync();
        }
    }
}
