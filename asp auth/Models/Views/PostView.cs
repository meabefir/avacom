using asp_auth.Models.DTOs;
using asp_auth.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Models.Views
{
    public class PostView
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CommentView> Comments { get; set; }
    }
}
