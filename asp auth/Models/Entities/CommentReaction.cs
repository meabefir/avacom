using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Models.Entities
{
    public class CommentReaction
    {
        public int Id { get; set; }
        public int? CommentId { get; set; }
        public int? UserId { get; set; }
        public string ReactionType { get; set; }

        [ForeignKey("CommentId")]
        public Comment Comment { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
