using asp_auth.Models;
using asp_auth.Models.DTOs;
using asp_auth.Models.Entities;
using asp_auth.Models.Views;
using lab2.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PostView = asp_auth.Models.Views.PostView;

public class DateComparer : IComparer<DateTime>
{
    // Call CaseInsensitiveComparer.Compare with the parameters reversed.
    public int Compare(DateTime x, DateTime y)
    {
        return 1;
    }
}

class Test
{
    public Post post { get; set; }
    public Friend friend { get; set; }
    public string Username { get; set; }
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

            var log = _context.Friends
                .ToList()
                .Where(f => f.User1Id == userId || f.User2Id == userId)
                .Join(
                    _context.Posts.Include(p => p.User).ThenInclude(u => u.Avatar),
                    f => f.User1Id == userId ? f.User2Id : f.User1Id,
                    post => post.UserId,
                    (f, post) => new Test
                    {
                        post = post,
                        friend = f,
                    }
                )
                .GroupJoin(
                    _context.Comments.Include(c => c.User),
                    pf => pf.post,
                    comms => comms.Post,
                    (pf, comms) => new PostView
                    {
                        IsMyPost = pf.post.UserId == userId,
                        Reactions = _context.PostReactions.Where(pr => pr.PostId.Equals(pf.post.Id))
                                                        .AsEnumerable().GroupBy(pr => pr.ReactionType).Select(gr => new ReactionGroupBy
                                                        {
                                                            Type = gr.Key,
                                                            Count = gr.Count()
                                                        }).ToList(),
                        LikedByMe = _context.PostReactions.Where(pr => pr.PostId.Equals(pf.post.Id))
                                                            .Where(pr => pr.UserId.Equals(userId))
                                                            .Where(pr => pr.ReactionType.Equals("like"))
                                                            .ToList().Count > 0,
                        DislikedByMe = _context.PostReactions.Where(pr => pr.PostId.Equals(pf.post.Id))
                                                            .Where(pr => pr.UserId.Equals(userId))
                                                            .Where(pr => pr.ReactionType.Equals("dislike"))
                                                            .ToList().Count > 0,
                        Id = pf.post.Id,
                        Title = pf.post.Title,
                        Text = pf.post.Text,
                        Username = pf.post.User.UserName,
                        Avatar = new AvatarView
                        {
                            EyesId = pf.post.User.Avatar.EyesId,
                            BodyId = pf.post.User.Avatar.BodyId,
                            ClothingId = pf.post.User.Avatar.ClothingId,
                            NoseId = pf.post.User.Avatar.NoseId,
                            BrowsId = pf.post.User.Avatar.BrowsId,
                            LipsId = pf.post.User.Avatar.LipsId,
                            HairId = pf.post.User.Avatar.HairId,
                        },
                        CreatedAt = pf.post.CreatedAt,
                        Comments = comms.Select(c => new CommentView
                        {
                            Username = c.User.UserName,
                            Text = c.Text,
                            CreatedAt = c.CreatedAt
                        }).ToList()
                    }
                )
            //.OrderBy(pw => pw.CreatedAt, new DateComparer()) doesnt work for some fucked up reason ahah
            .OrderBy(pw => pw.CreatedAt)
            .ToList();
            // .ToListAsync();

            //var log = _context.Posts.ToList()
            //    .GroupJoin(
            //        _context.Comments,
            //        pf => pf,
            //        comms => comms.Post,
            //        (pf, comms) => new
            //        {
            //            Id = pf.Id,
            //            Title = pf.Title,
            //            Text = pf.Text,
            //            CreatedAt = pf.CreatedAt,
            //            Comments = comms.Select(c => new
            //            {
            //                Username = c.User.UserName,
            //                Text = c.Text,
            //                CreatedAt = c.CreatedAt
            //            }).ToList()
            //        }
            //    );
            //foreach (var obj in log)
            //{
            //    Console.WriteLine("{0}: {1}", obj.Id, obj.Comments.Count());
            //}

            try
            {
                _context.Friends.Remove(temp_f);
                _context.SaveChanges();
            }
            catch (Exception e)
            { }

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
