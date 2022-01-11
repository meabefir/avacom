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
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostReaction> PostReactions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentReaction> CommentReactions { get; set; }

        // protected override void OnModelCreating(ModelBuilder builder)
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    relationship.DeleteBehavior = DeleteBehavior.Restrict;
            //}

            builder.Entity<UserRole>(ur =>
            {
                ur.HasKey(ur => new { ur.UserId, ur.RoleId });
                ur.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId);
                ur.HasOne(ur => ur.User).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.UserId);
            });

            // one to one intre user si avatar
            builder.Entity<User>()
                .HasOne(u => u.Avatar)
                .WithOne(a => a.User);

            // one to one intre user si user_profile
            builder.Entity<User>()
                .HasOne(u => u.UserProfile)
                .WithOne(up => up.User);

            // many to many intre friend si user
            //builder.Entity<Friend>(f =>
            //{
            //    f.HasKey(f => new { f.User1Id, f.User2Id });
            //    //f.HasOne(f => f.User1).WithMany(u => u.Friends);
            //    //f.HasOne(f => f.User2).WithMany(u => u.FriendsImWith);
            //    f.HasOne(f => f.User1).WithMany(u => u.Friends).HasForeignKey(f => f.User1Id).OnDelete(DeleteBehavior.NoAction);
            //    f.HasOne(f => f.User2).WithMany(u => u.FriendsImWith).HasForeignKey(f => f.User2Id).OnDelete(DeleteBehavior.NoAction);
            //});

            //builder.Entity<Friend>()
            //    .HasKey(f => new { f.User1Id, f.User2Id });
            //builder.Entity<Friend>()
            //        .HasOne(f => f.User1)
            //        .WithMany(u => u.Friends);
            //builder.Entity<Friend>()
            //        .HasOne(f => f.User2)
            //        .WithMany(u => u.FriendsImWith);


            // one to many intre user si postare
            builder.Entity<User>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.User);

            // o to m intre post si post_react si o to m intre user si post_react
            //builder.Entity<PostReaction>(pr =>
            //{
            //    pr.HasKey(pr => new { pr.PostId, pr.UserId });
            //    pr.HasOne(pr => pr.Post).WithMany(p => p.PostReactions);
            //    pr.HasOne(pr => pr.User).WithMany(u => u.PostReactions);
            //});

            // one to many intre postare si comm
            builder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Post);
            //   .HasForeignKey(p => p.PostId);

            // o to m intre comm si comm_react si o to m intre user si comm_react
            //builder.Entity<CommentReaction>(cr =>
            //{
            //    cr.HasKey(cr => new { cr.CommentId, cr.UserId });
            //    cr.HasOne(cr => cr.Comment).WithMany(c => c.CommentReactions);
            //    cr.HasOne(cr => cr.User).WithMany(u => u.CommentReactions);
            //});
        }
    }
}
