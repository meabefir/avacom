using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Models.Views
{
    public class FriendRequestView
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public DateTime SentAt { get; set; }
    }
}
