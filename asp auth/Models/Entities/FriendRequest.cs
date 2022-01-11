using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Models.Entities
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public int? SenderId { get; set; }
        public int? ReceiverId { get; set; }
        public DateTime SentAt { get; set; }

        [ForeignKey("SenderId"), Column(Order = 0)]
        public User Sender { get; set; }

        [ForeignKey("ReceiverId"), Column(Order = 1)]
        public User Receiver { get; set; }
    }
}
