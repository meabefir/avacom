using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Models.Entities
{
    public class Friend
    {
        public int Id { get; set; }
        public int? User1Id { get; set; }
        public int? User2Id { get; set; }
        public DateTime FriendsSince { get; set; }

        [ForeignKey("User1Id"), Column(Order = 0)]
        public User User1 { get; set; }

        [ForeignKey("User2Id"), Column(Order = 1)]
        public User User2 { get; set; }
    }
}
