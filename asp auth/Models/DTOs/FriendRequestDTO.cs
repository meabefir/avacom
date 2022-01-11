using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Models.DTOs
{
    public class FriendRequestDTO
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
    }
}
