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
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<List<PostView>> GetPostsByUserId(int userId)
        {
            // return await _context.Posts.Where(p => p.UserId.Equals(userId)).ToListAsync();

            return await _context.Users
                .Where(u => u.Id.Equals(userId))
                .Join(
                    _context.Posts,
                    user => user.Id,
                    post => post.User.Id,
                    (user, post) =>
                        new PostView
                        {
                            Title = post.Title,
                            Text = post.Text,
                            Username = user.UserName,
                            CreatedAt = post.CreatedAt
                        }
                )
                .Where(p => p.CreatedAt.Date.Equals(DateTime.Today))
                .ToListAsync();

            
            // return await _context.Posts.Include(p => p.User)
            //return await _context.Posts
            //                .Where(p => p.User.Id.Equals(userId))
            //                .Where(p => p.CreatedAt.Date.Equals(DateTime.Today))
            //                .Select(p => new PostView
            //                {
            //                    Title = p.Title,
            //                    Text = p.Text,
            //                    Username = p.User.UserName,
            //                    CreatedAt = p.CreatedAt
            //                })
            //                //.Take(2)
            //                .ToListAsync();
        }
    }
}
