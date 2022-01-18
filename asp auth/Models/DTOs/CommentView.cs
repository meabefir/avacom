using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Models.DTOs
{
    public class CommentView
    {
        public string Username { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
