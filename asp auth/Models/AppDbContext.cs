using asp_auth.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Models
{
    public class AppDbContext : IdentityDbContext<User, Role, int,
        IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<SessionToken> SessionTokens { get; set; }

        public override DbSet<User> Users { get; set; }

        public DbSet<Avatar> Avatars { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostReaction> PostReactions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentReaction> CommentReactions { get; set; }

        // protected override void OnModelCreating(ModelBuilder builder)
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRole>(ur =>
            {
                ur.HasKey(ur => new { ur.UserId, ur.RoleId });
                ur.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId);
                ur.HasOne(ur => ur.User).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.UserId);
            });

            // one to one intre user si avatar
            builder.Entity<Avatar>().HasKey(a => a.UserId);
            builder.Entity<Avatar>()
                .HasOne(a => a.User)
                .WithOne(u => u.Avatar);

            // one to one intre user si user_profile
            builder.Entity<UserProfile>().HasKey(up => up.UserId);
            builder.Entity<UserProfile>()
                .HasOne(up => up.User)
                .WithOne(u => u.UserProfile);

            // many to many intre friend si user
            builder.Entity<Friend>(f =>
            {
                f.HasKey(f => new { f.User1Id, f.User2Id });
                f.HasOne(f => f.User1).WithMany(u => u.Friends).HasForeignKey(f => f.User1Id).OnDelete(DeleteBehavior.NoAction);
                f.HasOne(f => f.User2).WithMany(u => u.FriendsImWith).HasForeignKey(f => f.User2Id).OnDelete(DeleteBehavior.NoAction);
            });

            // many to many intre friend_request si user
            builder.Entity<FriendRequest>(f =>
            {
                f.HasKey(f => new { f.SenderId, f.ReceiverId });
                f.HasOne(f => f.Sender).WithMany(u => u.FRReceivedFrom).HasForeignKey(f => f.SenderId).OnDelete(DeleteBehavior.NoAction);
                f.HasOne(f => f.Receiver).WithMany(u => u.FRSentTo).HasForeignKey(f => f.ReceiverId).OnDelete(DeleteBehavior.NoAction);
            });

            // one to many intre user si postare
            builder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId);

            // o to m intre post si post_react si o to m intre user si post_react
            builder.Entity<PostReaction>(pr =>
            {
                pr.HasKey(pr => new { pr.PostId, pr.UserId });
                pr.HasOne(pr => pr.Post).WithMany(p => p.PostReactions).HasForeignKey(pr => pr.PostId).OnDelete(DeleteBehavior.NoAction);
                pr.HasOne(pr => pr.User).WithMany(u => u.PostReactions).HasForeignKey(pr => pr.UserId).OnDelete(DeleteBehavior.NoAction);
            });

            // one to many intre comm si postare
            builder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.NoAction);
            // one to many intre comm si user
            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // o to m intre comm si comm_react si o to m intre user si comm_react
            builder.Entity<CommentReaction>(cr =>
            {
                cr.HasKey(cr => new { cr.CommentId, cr.UserId });
                cr.HasOne(cr => cr.Comment).WithMany(c => c.CommentReactions).HasForeignKey(pr => pr.CommentId).OnDelete(DeleteBehavior.NoAction);
                cr.HasOne(cr => cr.User).WithMany(u => u.CommentReactions).HasForeignKey(pr => pr.UserId).OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
