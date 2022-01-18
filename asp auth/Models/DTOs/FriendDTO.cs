using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Models.DTOs
{
    public class FriendDTO
    {
        public int User1_id { get; set; }
        public int User2_id { get; set; }
        public DateTime FriendsSince { get; set; }
    }
}
