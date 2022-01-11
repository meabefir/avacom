using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Models.DTOs
{
    public class CreatePostReactionDTO
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string ReactionType { get; set; }
    }
}
