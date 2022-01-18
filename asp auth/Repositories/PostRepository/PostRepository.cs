using asp_auth.Models;
using asp_auth.Models.Entities;
using asp_auth.Models.Views;
using lab2.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class DateComparer : IComparer<DateTime>
{
    // Call CaseInsensitiveComparer.Compare with the parameters reversed.
    public int Compare(DateTime x, DateTime y)
    {
        return 1;
    }
}

namespace asp_auth.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<List<PostView>> GetFeed(int userId)
        {
            var sender_user = await _context.Users.Where(u => u.Id == userId).FirstAsync();

            // tmp add friend relationship between me and me
            var temp_f = new Friend
            {
                User1 = sender_user,
                User2 = sender_user,
                User1Id = userId,
                User2Id = userId
            };
            try
            {
                _context.Friends.Add(temp_f);
                _context.SaveChanges();
            }
            catch (Exception e)
            {

            }

            var log = await _context.Friends
                .Where(f => f.User1Id == userId || f.User2Id == userId)
                .Join(
                    _context.Posts,
                    f => f.User1Id == userId ? f.User2Id : f.User1Id,
                    post => post.UserId,
                    (f, post) => new PostView
                    {
                        Id = post.Id,
                        Title = post.Title,
                        Text = post.Text,
                        Username = f.User1Id == userId ? f.User2.UserName : f.User1.UserName,
                        CreatedAt = post.CreatedAt
                    }
                )
                //.OrderBy(pw => pw.CreatedAt, new DateComparer()) doesnt work for some fucked up reason ahah
                .OrderBy(pw => pw.CreatedAt)
                .ToListAsync();
            _context.Friends.Remove(temp_f);
            _context.SaveChanges();
            return log;
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
                            Id = post.Id,
                            Title = post.Title,
                            Text = post.Text,
                            Username = user.UserName,
                            CreatedAt = post.CreatedAt
                        }
                )
                .Where(p => p.CreatedAt.AddDays(7) >= DateTime.Today)
                .ToListAsync();
        }
    }
}
