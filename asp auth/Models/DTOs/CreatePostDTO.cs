﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Models.DTOs
{
    public class CreatePostDTO
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

    }
}
