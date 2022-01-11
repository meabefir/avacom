using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Models.Entities
{
    public class User: IdentityUser<int>
    {
        public User(): base()
        {

        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }

        [InverseProperty("User")]
        public Avatar Avatar { get; set; }

        [InverseProperty("User")]
        public UserProfile UserProfile { get; set; }

        [InverseProperty("User1")]
        public ICollection<Friend> Friends { get; set; }

        [InverseProperty("User2")]
        public ICollection<Friend> FriendsImWith { get; set; }

        [InverseProperty("Sender")]
        public ICollection<FriendRequest> FRReceivedFrom { get; set; }
        
        [InverseProperty("Receiver")]
        public ICollection<FriendRequest> FRSentTo { get; set; }

        [InverseProperty("User")]
        public ICollection<Post> Posts { get; set; }

        [InverseProperty("User")]
        public ICollection<Comment> Comments { get; set; }

        [InverseProperty("User")]
        public ICollection<PostReaction> PostReactions { get; set; }

        [InverseProperty("User")]
        public ICollection<CommentReaction> CommentReactions { get; set; }

    }
}
