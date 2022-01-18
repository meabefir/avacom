using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Models.DTOs
{
    public class CommentDTO
    {
        public int PostId { get; set; }
        public string Text { get; set; }
    }
}
