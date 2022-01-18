using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Models.DTOs
{
    public class FriendView
    {
        public string Username { get; set; }
        public DateTime FriendsSince { get; set; }
    }
}
