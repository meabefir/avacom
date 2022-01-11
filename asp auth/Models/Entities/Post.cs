using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Models.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [InverseProperty("Post")]
        public ICollection<PostReaction> PostReactions { set; get; }

        [InverseProperty("Post")]
        public ICollection<Comment> Comments { set; get; }
    }
}
